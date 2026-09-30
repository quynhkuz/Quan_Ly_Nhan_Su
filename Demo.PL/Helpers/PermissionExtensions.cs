using System.Security.Claims;

namespace Demo.PL.Helpers
{
    /// <summary>
    /// Câu hỏi quyền theo cấp bậc: Admin &gt; Editor &gt; User.
    /// </summary>
    public static class PermissionExtensions
    {
        /// <summary>Cấp cao nhất: toàn quyền, gồm quản lý người dùng.</summary>
        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(Roles.Admin);

        /// <summary>Được thêm / sửa / xoá nhân viên và phòng ban (Admin hoặc Editor).</summary>
        public static bool CanEditContent(this ClaimsPrincipal user)
            => user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Editor);

        /// <summary>Được quản lý người dùng và gán quyền cho người khác (chỉ Admin).</summary>
        public static bool CanManageUsers(this ClaimsPrincipal user) => user.IsInRole(Roles.Admin);
    }
}
