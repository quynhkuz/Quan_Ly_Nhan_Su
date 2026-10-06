using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly EmailSettings _emailSettings;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailSettings emailSettings)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSettings = emailSettings;
		}

        #region Sign Up

        public IActionResult SignUp() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        { 
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is not null)
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập này đã tồn tại :(");
                }
                else
                {
                    user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = true,
                        FName = model.FName,
                        LName = model.LName

                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Helpers.Roles.Admin);
                        return RedirectToAction(nameof(SignIn));
                    }

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        #endregion

        #region Sign In

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
					}
                }
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng");
            }
            return View(model);
        }

        #endregion

        #region Sign Out

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordURL(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);


                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipients = user.Email,
                        Body = resetPasswordUrl
                    };
                    try
                    {
                        _emailSettings.SendEmail(email);
                        return RedirectToAction(nameof(CheckYourInbox));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Không gửi được email: {ex.Message}");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email không tồn tại");
                }
            }
            return View(nameof(ForgetPassword), model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Yêu cầu đặt lại mật khẩu không hợp lệ");
                    return View(model);
                }

                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                    return RedirectToAction(nameof(SignIn));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
		#endregion

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
	}
}
