using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(int? payrollPeriodId, int? departmentId)
        {
            var periods = _unitOfWork.PayrollRepository.GetAllOrdered();
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            var filter = new ReportFilterViewModel
            {
                PayrollPeriodId = payrollPeriodId,
                DepartmentId = departmentId,
                Periods = _mapper.Map<IEnumerable<PayrollPeriod>, IEnumerable<PayrollPeriodViewModel>>(periods),
                Departments = departments
            };

            // Mặc định chọn kỳ lương mới nhất nếu chưa chọn
            var period = payrollPeriodId.HasValue
                ? periods.FirstOrDefault(P => P.Id == payrollPeriodId.Value)
                : periods.FirstOrDefault();

            if (period is null)
            {
                return View(new ReportViewModel { Filter = filter });
            }

            filter.PayrollPeriodId = period.Id;

            var payslips = _unitOfWork.PayrollRepository.GetPayslipsByPeriod(period.Id).AsEnumerable();

            var departmentName = "Tất cả phòng ban";
            if (departmentId.HasValue)
            {
                payslips = payslips.Where(S => S.Employee.DepartmentId == departmentId.Value);
                departmentName = departments.FirstOrDefault(D => D.Id == departmentId.Value)?.Name ?? "Không xác định";
            }

            var list = payslips.ToList();

            var model = new ReportViewModel
            {
                Filter = filter,
                PeriodName = $"{period.Month}/{period.Year}",
                DepartmentName = departmentName,
                Payslips = _mapper.Map<IEnumerable<Payslip>, IEnumerable<PayslipViewModel>>(list),
                EmployeeCount = list.Count,
                TotalBaseSalary = list.Sum(S => S.BaseSalary),
                TotalAllowance = list.Sum(S => S.Allowance),
                TotalBonus = list.Sum(S => S.Bonus),
                TotalDeduction = list.Sum(S => S.Deduction),
                TotalTax = list.Sum(S => S.Tax),
                TotalNetSalary = list.Sum(S => S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax)
            };

            return View(model);
        }

        [Authorize(Roles = Roles.CanEditContent)]
        public IActionResult ExportExcel(int? payrollPeriodId, int? departmentId)
        {
            var periods = _unitOfWork.PayrollRepository.GetAllOrdered();
            var period = payrollPeriodId.HasValue
                ? periods.FirstOrDefault(P => P.Id == payrollPeriodId.Value)
                : periods.FirstOrDefault();

            if (period is null)
            {
                TempData["Message"] = "Chưa có kỳ lương để xuất báo cáo.";
                return RedirectToAction(nameof(Index));
            }

            var payslips = _unitOfWork.PayrollRepository.GetPayslipsByPeriod(period.Id).AsEnumerable();

            var departmentName = "TatCaPhongBan";
            if (departmentId.HasValue)
            {
                payslips = payslips.Where(S => S.Employee.DepartmentId == departmentId.Value);
                var dep = _unitOfWork.DepartmentRepository.Get(departmentId.Value);
                departmentName = dep?.Name.Replace(" ", "") ?? "PhongBan";
            }

            var list = payslips.OrderBy(S => S.Employee.Name).ToList();

            var headers = new List<string>
            {
                "Mã NV", "Họ và tên", "Phòng ban", "Lương cơ bản",
                "Phụ cấp", "Thưởng", "Khấu trừ", "Thuế", "Thực nhận"
            };

            var rows = list.Select(S => (IList<object>)new object[]
            {
                S.EmployeeId,
                S.Employee?.Name ?? "",
                S.Employee?.Department?.Name ?? "",
                S.BaseSalary,
                S.Allowance,
                S.Bonus,
                S.Deduction,
                S.Tax,
                S.BaseSalary + S.Allowance + S.Bonus - S.Deduction - S.Tax
            }).ToList();

            var bytes = ExcelExporter.Export(
                "BaoCaoLuong",
                $"Báo cáo lương kỳ {period.Month}/{period.Year} - {departmentName}",
                headers,
                rows);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"BaoCaoLuong-{period.Month}-{period.Year}-{departmentName}.xlsx");
        }
    }
}
