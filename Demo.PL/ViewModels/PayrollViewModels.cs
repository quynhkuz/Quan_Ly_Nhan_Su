using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Demo.DAL.Models;

namespace Demo.PL.ViewModels
{
    public class PayrollPeriodViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kỳ lương")]
        public string Name { get; set; }

        [Display(Name = "Tháng")]
        public int Month { get; set; }

        [Display(Name = "Năm")]
        public int Year { get; set; }

        [Display(Name = "Trạng thái")]
        public PayrollStatus Status { get; set; }

        [Display(Name = "Ngày tính")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày chốt")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = "-")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "Tổng quỹ lương")]
        [DataType(DataType.Currency)]
        public decimal TotalNetSalary { get; set; }

        [Display(Name = "Số nhân viên")]
        public int EmployeeCount { get; set; }
    }

    public class CreatePayrollViewModel
    {
        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải từ 1 đến 12")]
        public int Month { get; set; } = DateTime.Now.Month;

        [Display(Name = "Năm")]
        [Range(2020, 2100, ErrorMessage = "Năm phải từ 2020 đến 2100")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Display(Name = "Phụ cấp (%)")]
        [Range(0, 100, ErrorMessage = "Phụ cấp phải từ 0 đến 100%")]
        public decimal AllowancePercent { get; set; } = 10;

        [Display(Name = "Thưởng (%)")]
        [Range(0, 100, ErrorMessage = "Thưởng phải từ 0 đến 100%")]
        public decimal BonusPercent { get; set; } = 0;

        [Display(Name = "Khấu trừ - BHXH, BHYT (%)")]
        [Range(0, 100, ErrorMessage = "Khấu trừ phải từ 0 đến 100%")]
        public decimal DeductionPercent { get; set; } = 10.5m;

        [Display(Name = "Thuế TNCN (%)")]
        [Range(0, 100, ErrorMessage = "Thuế phải từ 0 đến 100%")]
        public decimal TaxPercent { get; set; } = 10;
    }

    public class PayrollDetailsViewModel
    {
        public PayrollPeriodViewModel Period { get; set; }
        public IEnumerable<PayslipViewModel> Payslips { get; set; }
    }

    public class PayslipViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Mã nhân viên")]
        public int EmployeeId { get; set; }

        [Display(Name = "Họ và tên")]
        public string EmployeeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string DepartmentName { get; set; }

        [Display(Name = "Kỳ lương")]
        public string PeriodName { get; set; }

        public int PayrollPeriodId { get; set; }

        [Display(Name = "Lương cơ bản")]
        [DataType(DataType.Currency)]
        public decimal BaseSalary { get; set; }

        [Display(Name = "Phụ cấp")]
        [DataType(DataType.Currency)]
        public decimal Allowance { get; set; }

        [Display(Name = "Thưởng")]
        [DataType(DataType.Currency)]
        public decimal Bonus { get; set; }

        [Display(Name = "Khấu trừ")]
        [DataType(DataType.Currency)]
        public decimal Deduction { get; set; }

        [Display(Name = "Thuế")]
        [DataType(DataType.Currency)]
        public decimal Tax { get; set; }

        [Display(Name = "Lương thực nhận")]
        [DataType(DataType.Currency)]
        public decimal NetSalary { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
    }

    public class EditPayslipViewModel
    {
        public int Id { get; set; }
        public int PayrollPeriodId { get; set; }

        [Display(Name = "Họ và tên")]
        public string EmployeeName { get; set; }

        [Display(Name = "Lương cơ bản")]
        [DataType(DataType.Currency)]
        public decimal BaseSalary { get; set; }

        [Display(Name = "Phụ cấp")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000000, ErrorMessage = "Phụ cấp không hợp lệ")]
        public decimal Allowance { get; set; }

        [Display(Name = "Thưởng")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000000, ErrorMessage = "Thưởng không hợp lệ")]
        public decimal Bonus { get; set; }

        [Display(Name = "Khấu trừ")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000000, ErrorMessage = "Khấu trừ không hợp lệ")]
        public decimal Deduction { get; set; }

        [Display(Name = "Thuế")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000000, ErrorMessage = "Thuế không hợp lệ")]
        public decimal Tax { get; set; }

        [Display(Name = "Ghi chú")]
        [MaxLength(250)]
        public string Note { get; set; }
    }
}
