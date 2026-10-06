using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class DashboardViewModel
    {
        [Display(Name = "Tổng nhân viên")]
        public int TotalEmployees { get; set; }

        [Display(Name = "Đang làm việc")]
        public int ActiveEmployees { get; set; }

        [Display(Name = "Phòng ban")]
        public int TotalDepartments { get; set; }

        [Display(Name = "Kỳ lương đã chốt")]
        public int ClosedPeriods { get; set; }

        [Display(Name = "Tổng quỹ lương kỳ gần nhất")]
        [DataType(DataType.Currency)]
        public decimal LatestPeriodTotal { get; set; }

        [Display(Name = "Kỳ lương gần nhất")]
        public string LatestPeriodName { get; set; }

        [Display(Name = "Lương trung bình")]
        [DataType(DataType.Currency)]
        public decimal AverageSalary { get; set; }

        public IEnumerable<DepartmentSalaryStat> SalaryByDepartment { get; set; }
            = new List<DepartmentSalaryStat>();

        public IEnumerable<PayrollPeriodViewModel> LatestPeriods { get; set; }
            = new List<PayrollPeriodViewModel>();
    }

    public class DepartmentSalaryStat
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal TotalSalary { get; set; }
    }
}
