using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Models
{
    public enum PayrollStatus
    {
        Draft = 0,
        Closed = 1
    }

    public class PayrollPeriod : ModelBase
    {
        [Display(Name = "Tháng")]
        public int Month { get; set; }

        [Display(Name = "Năm")]
        public int Year { get; set; }

        [Display(Name = "Tên kỳ lương")]
        public string Name => $"{Month}/{Year}";

        [Display(Name = "Trạng thái")]
        public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

        [Display(Name = "Ngày tính")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày chốt")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "Tổng quỹ lương")]
        public decimal TotalNetSalary { get; set; }

        public ICollection<Payslip> Payslips { get; set; } = new List<Payslip>();
    }
}
