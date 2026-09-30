using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Models
{
    public class Department : ModelBase
    {

        [Display(Name = "Mã phòng ban")]
        [Required(ErrorMessage = "Mã phòng ban là bắt buộc")]
        [MaxLength(10, ErrorMessage = "Mã phòng ban không được vượt quá 10 ký tự")]
        public string Code { get; set; }

        [Display(Name = "Tên phòng ban")]
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên phòng ban không được vượt quá 50 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
