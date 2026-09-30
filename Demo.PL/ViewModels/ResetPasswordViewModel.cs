using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [Compare(nameof(NewPassword), ErrorMessage = "Hai mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
