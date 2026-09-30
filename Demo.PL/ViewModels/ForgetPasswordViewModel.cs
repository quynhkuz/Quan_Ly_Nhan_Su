using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[Display(Name = "Email")]
		[Required(ErrorMessage = "Email là bắt buộc")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }
	}
}
