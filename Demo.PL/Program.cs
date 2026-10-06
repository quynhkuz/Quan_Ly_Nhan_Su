using Demo.DAL.Data.Context;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        // Tài khoản này luôn được gán quyền Admin khi khởi động.
        private const string AdminEmail = "quynh@test.com";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedRoles(host);

            host.Run();
        }

        private static void SeedRoles(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            services.GetRequiredService<AppDbContext>().Database.Migrate();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { Roles.Admin, Roles.Editor, Roles.User })
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }

            // Tài khoản này luôn có đúng một quyền: Admin (gỡ bỏ quyền cũ nếu có).
            var admin = userManager.FindByEmailAsync(AdminEmail).Result;

            if (admin is not null)
            {
                var currentRoles = userManager.GetRolesAsync(admin).Result;

                if (currentRoles.Count != 1 || currentRoles[0] != Roles.Admin)
                {
                    foreach (var oldRole in currentRoles)
                        _ = userManager.RemoveFromRoleAsync(admin, oldRole).Result;

                    _ = userManager.AddToRoleAsync(admin, Roles.Admin).Result;
                }
            }

            // Tất cả tài khoản đăng ký đều là Admin
            foreach (var user in userManager.Users.ToList())
            {
                if (userManager.GetRolesAsync(user).Result.Count == 0)
                    _ = userManager.AddToRoleAsync(user, Roles.Admin).Result;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
