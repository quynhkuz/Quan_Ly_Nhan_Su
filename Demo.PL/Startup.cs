using Demo.BLL.Interfaces;
using Demo.BLL.Reopsitories;
using Demo.DAL.Data.Context;
using Demo.DAL.Models;
using Demo.PL.Extentions;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(name => $"Trường {name} phải là số.");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Giá trị phải là số.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(value => $"Giá trị '{value}' không hợp lệ.");
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, name) => $"Giá trị '{value}' không hợp lệ cho trường {name}.");
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(name => $"Giá trị của trường {name} không hợp lệ.");
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Giá trị không hợp lệ.");
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(name => $"Trường {name} không được để trống.");
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(name => $"Thiếu giá trị của trường {name}.");
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Thiếu giá trị.");
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "Thiếu dữ liệu trong yêu cầu.");
            });
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            },ServiceLifetime.Scoped);
            
            services.AddAplicationServices();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 2;
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireNonAlphanumeric = true;
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }
            ).AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders()
             .AddErrorDescriber<VietnameseIdentityErrorDescriber>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                config.SlidingExpiration = true;
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();

            var supportedCultures = new[] { "vi-VN", "en-US" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("vi-VN")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
