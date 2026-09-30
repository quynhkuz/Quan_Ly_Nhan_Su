using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class SignUpViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
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


        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [Compare(nameof(Password), ErrorMessage = "Hai mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Đồng ý với điều khoản")]
        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản")]
        public bool IsAgree { get; set; }
    }
}
