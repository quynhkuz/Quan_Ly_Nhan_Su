using System;
using System.Collections.Generic;

namespace Demo.PL.Helpers
{
    /// <summary>
    /// Đổi tên tiếng Anh (giá trị lưu trong dữ liệu) sang tiếng Việt khi hiển thị.
    /// Giá trị gốc trong DB không thay đổi.
    /// </summary>
    public static class DisplayNames
    {
        private static readonly Dictionary<string, string> Departments =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Information Technology"] = "Công nghệ thông tin",
                ["Human Resources"] = "Nhân sự",
                ["Sales"] = "Kinh doanh",
                ["Marketing"] = "Marketing",
                ["Finance"] = "Tài chính",
            };

        private static readonly Dictionary<string, string> Roles =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Admin"] = "Quản trị viên",
                ["Editor"] = "Biên tập viên",
                ["Viewer"] = "Người xem",
                ["User"] = "Người dùng",
                ["Employee"] = "Nhân viên",
                ["HR"] = "Nhân sự",
            };

        public static string Department(string name)
            => name != null && Departments.TryGetValue(name, out var vietnamese) ? vietnamese : name;

        public static string Role(string name)
            => name != null && Roles.TryGetValue(name, out var vietnamese) ? vietnamese : name;
    }
}
