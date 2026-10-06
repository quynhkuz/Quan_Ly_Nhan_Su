using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ReportFilterViewModel
    {
        [Display(Name = "Kỳ lương")]
        public int? PayrollPeriodId { get; set; }

        [Display(Name = "Phòng ban")]
        public int? DepartmentId { get; set; }

        public IEnumerable<PayrollPeriodViewModel> Periods { get; set; }
            = new List<PayrollPeriodViewModel>();

        public IEnumerable<Department> Departments { get; set; }
            = new List<Department>();
    }

    public class ReportViewModel
    {
        public ReportFilterViewModel Filter { get; set; }

        public string PeriodName { get; set; }

        public string DepartmentName { get; set; }

        public IEnumerable<PayslipViewModel> Payslips { get; set; }
            = new List<PayslipViewModel>();

        [Display(Name = "Tổng lương cơ bản")]
        [DataType(DataType.Currency)]
        public decimal TotalBaseSalary { get; set; }

        [Display(Name = "Tổng phụ cấp")]
        [DataType(DataType.Currency)]
        public decimal TotalAllowance { get; set; }

        [Display(Name = "Tổng thưởng")]
        [DataType(DataType.Currency)]
        public decimal TotalBonus { get; set; }

        [Display(Name = "Tổng khấu trừ")]
        [DataType(DataType.Currency)]
        public decimal TotalDeduction { get; set; }

        [Display(Name = "Tổng thuế")]
        [DataType(DataType.Currency)]
        public decimal TotalTax { get; set; }

        [Display(Name = "Tổng thực nhận")]
        [DataType(DataType.Currency)]
        public decimal TotalNetSalary { get; set; }

        [Display(Name = "Số nhân viên")]
        public int EmployeeCount { get; set; }
    }
}
