using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        [Display(Name = "Mã nhân viên")]
        public int Id { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        [MinLength(5, ErrorMessage = "Họ và tên phải có ít nhất 5 ký tự")]
        public string Name { get; set; }


        [Display(Name = "Tuổi")]
        [Range(22, 30, ErrorMessage = "Tuổi phải từ 22 đến 30")]
        public int? Age { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Lương")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public decimal Salary { get; set; }


        [Display(Name = "Đang làm việc")]
        public bool IsActive { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ngày vào làm")]
        [Required(ErrorMessage = "{0} là bắt buộc")]
        public DateTime HiringDate { get; set; }

        [Display(Name = "Ảnh")]
        public IFormFile Image { get; set; }

        [Display(Name = "Tên file ảnh")]
        public string ImageName { get; set; }

        [Display(Name = "Mã phòng ban")]
        public int? DepartmentId { get; set; } // Forgin Key Column

        [Display(Name = "Phòng ban")]
        public Department Department { get; set; }

    }
}
