namespace Demo.PL.Helpers
{
    public static class Roles
    {
        /// <summary>Cấp cao nhất: toàn quyền, gồm quản lý người dùng.</summary>
        public const string Admin = "Admin";

        /// <summary>Cấp giữa: được thêm / sửa / xoá nhân viên và phòng ban.</summary>
        public const string Editor = "Editor";

        /// <summary>Cấp thấp nhất: chỉ xem, không thay đổi dữ liệu.</summary>
        public const string User = "User";

        /// <summary>Các cấp được phép thêm / sửa / xoá nhân viên và phòng ban.</summary>
        public const string CanEditContent = Admin + "," + Editor;

        /// <summary>Các cấp được phép quản lý người dùng.</summary>
        public const string CanManageUsers = Admin;
    }
}
