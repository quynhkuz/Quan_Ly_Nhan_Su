using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class UserViewModel
	{
		[Display(Name = "Mã người dùng")]
		public string Id { get; set; }

		[Display(Name = "Tên đăng nhập")]
		public string UserName { get; set; }

		[Display(Name = "Tên")]
		[Required(ErrorMessage = "Tên là bắt buộc")]
		public string FName { get; set; }

		[Display(Name = "Họ")]
		[Required(ErrorMessage = "Họ là bắt buộc")]
		public string LName { get; set; }

		[Display(Name = "Email")]
		[Required(ErrorMessage = "Email là bắt buộc")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }

		[Display(Name = "Số điện thoại")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Vai trò")]
		public IEnumerable<string> Roles { get; set; }

		[Display(Name = "Phân quyền")]
		public string Role { get; set; }

        public UserViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }

    }

}
