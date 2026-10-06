using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class PayrollController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PayrollController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Danh sách kỳ lương

        public IActionResult Index()
        {
            var periods = _unitOfWork.PayrollRepository.GetAllOrdered();
            var mapped = _mapper.Map<IEnumerable<PayrollPeriod>, IEnumerable<PayrollPeriodViewModel>>(periods);
            return View(mapped);
        }

        #endregion

        #region Tính lương

        [Authorize(Roles = Roles.CanEditContent)]
        public IActionResult Create()
        {
            return View(new CreatePayrollViewModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.CanEditContent)]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePayrollViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_unitOfWork.PayrollRepository.GetByMonthYear(model.Month, model.Year) is not null)
            {
                ModelState.AddModelError(nameof(model.Month), $"Kỳ lương {model.Month}/{model.Year} đã tồn tại.");
                return View(model);
            }

            var employees = _unitOfWork.EmployeeRepository.GetAll()
                .Where(E => E.IsActive && !E.IsDeleted)
                .ToList();

            if (!employees.Any())
            {
                ModelState.AddModelError(string.Empty, "Không có nhân viên đang làm việc để tính lương.");
                return View(model);
            }

            try
            {
                var period = new PayrollPeriod
                {
                    Month = model.Month,
                    Year = model.Year,
                    Status = PayrollStatus.Draft,
                    CreatedDate = DateTime.Now
                };

                foreach (var employee in employees)
                {
                    var allowance = Math.Round(employee.Salary * model.AllowancePercent / 100, 0);
                    var bonus = Math.Round(employee.Salary * model.BonusPercent / 100, 0);
                    var deduction = Math.Round(employee.Salary * model.DeductionPercent / 100, 0);
                    var gross = employee.Salary + allowance + bonus - deduction;
                    var tax = Math.Round(gross * model.TaxPercent / 100, 0);

                    period.Payslips.Add(new Payslip
                    {
                        EmployeeId = employee.Id,
                        BaseSalary = employee.Salary,
                        Allowance = allowance,
                        Bonus = bonus,
                        Deduction = deduction,
                        Tax = tax,
                        Note = $"Phụ cấp {model.AllowancePercent}% - Khấu trừ {model.DeductionPercent}% - Thuế {model.TaxPercent}%",
                        CreatedDate = DateTime.Now
                    });
                }

                period.TotalNetSalary = period.Payslips.Sum(S => S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax);

                _unitOfWork.PayrollRepository.Add(period);
                _unitOfWork.Complete();

                TempData["Message"] = $"Đã tính lương kỳ {model.Month}/{model.Year} cho {employees.Count} nhân viên.";
                return RedirectToAction(nameof(Details), new { id = period.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        #endregion

        #region Bảng lương chi tiết

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var period = _unitOfWork.PayrollRepository.GetWithPayslips(id.Value);
            if (period is null)
                return NotFound();

            var viewModel = new PayrollDetailsViewModel
            {
                Period = _mapper.Map<PayrollPeriod, PayrollPeriodViewModel>(period),
                Payslips = _mapper.Map<IEnumerable<Payslip>, IEnumerable<PayslipViewModel>>(period.Payslips)
            };

            viewModel.Period.TotalNetSalary = period.Payslips.Sum(S => S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax);

            return View(viewModel);
        }

        #endregion

        #region Chốt bảng lương

        [HttpPost]
        [Authorize(Roles = Roles.CanEditContent)]
        [ValidateAntiForgeryToken]
        public IActionResult Close(int id)
        {
            var period = _unitOfWork.PayrollRepository.Get(id);
            if (period is null)
                return NotFound();

            if (period.Status == PayrollStatus.Closed)
            {
                TempData["Message"] = "Kỳ lương này đã được chốt trước đó.";
                return RedirectToAction(nameof(Details), new { id });
            }

            try
            {
                period.Status = PayrollStatus.Closed;
                period.ClosedDate = DateTime.Now;
                period.TotalNetSalary = _unitOfWork.PayrollRepository.GetTotalNetSalary(id);

                _unitOfWork.PayrollRepository.Update(period);
                _unitOfWork.Complete();

                TempData["Message"] = $"Đã chốt bảng lương kỳ {period.Month}/{period.Year}.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Chốt bảng lương thất bại: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        #endregion

        #region Phiếu lương

        public IActionResult Payslip(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var payslip = _unitOfWork.PayrollRepository.GetPayslip(id.Value);
            if (payslip is null)
                return NotFound();

            var viewModel = _mapper.Map<Payslip, PayslipViewModel>(payslip);
            return View(viewModel);
        }

        #endregion

        #region Điều chỉnh phiếu lương

        [Authorize(Roles = Roles.CanEditContent)]
        public IActionResult EditPayslip(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var payslip = _unitOfWork.PayrollRepository.GetPayslip(id.Value);
            if (payslip is null)
                return NotFound();

            if (payslip.PayrollPeriod.Status == PayrollStatus.Closed)
            {
                TempData["Message"] = "Kỳ lương đã chốt, không thể điều chỉnh.";
                return RedirectToAction(nameof(Payslip), new { id });
            }

            return View(_mapper.Map<Payslip, EditPayslipViewModel>(payslip));
        }

        [HttpPost]
        [Authorize(Roles = Roles.CanEditContent)]
        [ValidateAntiForgeryToken]
        public IActionResult EditPayslip(int id, EditPayslipViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var payslip = _unitOfWork.PayrollRepository.GetPayslip(id);
            if (payslip is null)
                return NotFound();

            if (payslip.PayrollPeriod.Status == PayrollStatus.Closed)
            {
                TempData["Message"] = "Kỳ lương đã chốt, không thể điều chỉnh.";
                return RedirectToAction(nameof(Payslip), new { id });
            }

            try
            {
                _mapper.Map(model, payslip);
                _unitOfWork.PayrollRepository.UpdatePayslip(payslip);
                _unitOfWork.Complete();

                var period = _unitOfWork.PayrollRepository.Get(payslip.PayrollPeriodId);
                if (period is not null && period.Status == PayrollStatus.Draft)
                {
                    period.TotalNetSalary = _unitOfWork.PayrollRepository.GetTotalNetSalary(payslip.PayrollPeriodId);
                    _unitOfWork.PayrollRepository.Update(period);
                    _unitOfWork.Complete();
                }

                TempData["Message"] = "Đã cập nhật phiếu lương.";
                return RedirectToAction(nameof(Payslip), new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        #endregion

        #region Xuất Excel bảng lương

        [Authorize(Roles = Roles.CanEditContent)]
        public IActionResult ExportExcel(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var period = _unitOfWork.PayrollRepository.GetWithPayslips(id.Value);
            if (period is null)
                return NotFound();

            var headers = new List<string>
            {
                "Mã NV", "Họ và tên", "Phòng ban", "Lương cơ bản",
                "Phụ cấp", "Thưởng", "Khấu trừ", "Thuế", "Thực nhận", "Ghi chú"
            };

            var rows = period.Payslips
                .OrderBy(S => S.Employee.Name)
                .Select(S => (IList<object>)new object[]
                {
                    S.EmployeeId,
                    S.Employee?.Name ?? "",
                    S.Employee?.Department?.Name ?? "",
                    S.BaseSalary,
                    S.Allowance,
                    S.Bonus,
                    S.Deduction,
                    S.Tax,
                    S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax,
                    S.Note ?? ""
                })
                .ToList();

            var status = period.Status == PayrollStatus.Closed ? "Đã chốt" : "Nháp";
            var bytes = ExcelExporter.Export(
                "BangLuong",
                $"Bảng lương kỳ {period.Month}/{period.Year} - {status}",
                headers,
                rows);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"BangLuong-{period.Month}-{period.Year}.xlsx");
        }

        #endregion
    }
}
