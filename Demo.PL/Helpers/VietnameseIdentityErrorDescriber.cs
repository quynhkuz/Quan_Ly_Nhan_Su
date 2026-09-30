using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helpers
{
    public class VietnameseIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
            => new IdentityError { Code = nameof(PasswordTooShort), Description = $"Mật khẩu phải có ít nhất {length} ký tự." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"Mật khẩu phải có ít nhất {uniqueChars} ký tự khác nhau." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt (ví dụ: @ # ! ?)." };

        public override IdentityError PasswordRequiresDigit()
            => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Mật khẩu phải có ít nhất 1 chữ số." };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Mật khẩu phải có ít nhất 1 chữ thường (a-z)." };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Mật khẩu phải có ít nhất 1 chữ hoa (A-Z)." };

        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError { Code = nameof(DuplicateUserName), Description = $"Tên đăng nhập \"{userName}\" đã tồn tại." };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email \"{email}\" đã được sử dụng cho tài khoản khác." };

        public override IdentityError InvalidUserName(string userName)
            => new IdentityError { Code = nameof(InvalidUserName), Description = $"Tên đăng nhập \"{userName}\" không hợp lệ." };

        public override IdentityError InvalidEmail(string email)
            => new IdentityError { Code = nameof(InvalidEmail), Description = $"Email \"{email}\" không hợp lệ." };

        public override IdentityError PasswordMismatch()
            => new IdentityError { Code = nameof(PasswordMismatch), Description = "Mật khẩu không đúng." };

        public override IdentityError InvalidToken()
            => new IdentityError { Code = nameof(InvalidToken), Description = "Liên kết không hợp lệ hoặc đã hết hạn. Vui lòng yêu cầu lại liên kết mới." };

        public override IdentityError RecoveryCodeRedemptionFailed()
            => new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Mã khôi phục không hợp lệ." };

        public override IdentityError LoginAlreadyAssociated()
            => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Tài khoản này đã được liên kết với một tài khoản khác." };

        public override IdentityError UserAlreadyInRole(string role)
            => new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Người dùng đã thuộc vai trò \"{role}\"." };

        public override IdentityError UserNotInRole(string role)
            => new IdentityError { Code = nameof(UserNotInRole), Description = $"Người dùng không thuộc vai trò \"{role}\"." };

        public override IdentityError UserLockoutNotEnabled()
            => new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Tài khoản của bạn đã bị khoá do đăng nhập sai quá nhiều lần." };

        public override IdentityError UserAlreadyHasPassword()
            => new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Người dùng đã có mật khẩu." };

        public override IdentityError ConcurrencyFailure()
            => new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Có lỗi xảy ra do thao tác chỉnh sửa cùng lúc với một thao tác khác. Vui lòng thử lại." };
    }
}
