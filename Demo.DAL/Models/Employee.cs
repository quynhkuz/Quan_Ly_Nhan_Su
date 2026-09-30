using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee : ModelBase
    {
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Display(Name = "Tuổi")]
        public int? Age { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Lương")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Đang làm việc")]
        public bool IsActive { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ngày vào làm")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HiringDate { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Display(Name = "Tên file ảnh")]
        public string ImageName { get; set; }

        [Display(Name = "Mã phòng ban")]
        public int? DepartmentId { get; set; } // Forgin Key Column

        [Display(Name = "Phòng ban")]
        public Department Department { get; set; }

    }
}
