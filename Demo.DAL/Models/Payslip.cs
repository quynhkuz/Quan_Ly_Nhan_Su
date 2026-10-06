using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Models
{
    public class Payslip : ModelBase
    {
        [Display(Name = "Kỳ lương")]
        public int PayrollPeriodId { get; set; }
        public PayrollPeriod PayrollPeriod { get; set; }

        [Display(Name = "Nhân viên")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Lương cơ bản")]
        public decimal BaseSalary { get; set; }

        [Display(Name = "Phụ cấp")]
        public decimal Allowance { get; set; }

        [Display(Name = "Thưởng")]
        public decimal Bonus { get; set; }

        [Display(Name = "Khấu trừ")]
        public decimal Deduction { get; set; }

        [Display(Name = "Thuế")]
        public decimal Tax { get; set; }

        [Display(Name = "Lương thực nhận")]
        public decimal NetSalary => BaseSalary + Allowance + Bonus - Deduction - Tax;

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Ngày tính")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
