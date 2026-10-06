using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class CreateUserViewModel
	{
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
		[Required(ErrorMessage = "Vai trò là bắt buộc")]
		public string Role { get; set; }

		[Display(Name = "Mật khẩu")]
		[Required(ErrorMessage = "Mật khẩu là bắt buộc")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Xác nhận mật khẩu")]
		[Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		public string ConfirmPassword { get; set; }
	}
}
