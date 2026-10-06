using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		private async Task<UserViewModel> MapToViewModel(ApplicationUser user)
		{
			var roles = await _userManager.GetRolesAsync(user);

			return new UserViewModel
			{
				Id = user.Id,
				UserName = user.UserName,
				FName = user.FName,
				LName = user.LName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Roles = roles,
				Role = roles.FirstOrDefault()
			};
		}

		public async Task<IActionResult> Index(string email)
		{
			if(string.IsNullOrEmpty(email))
			{
				var users = new List<UserViewModel>();
				foreach (var u in _userManager.Users)
				{
					users.Add(await MapToViewModel(u));
				}
				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(email);

				if (user is null)
					return NotFound();

				return View(new List<UserViewModel>() { await MapToViewModel(user) });
			}
		}

		public async Task<IActionResult> Details(string id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			return View(await MapToViewModel(user));
		}

		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Create()
		{
			await FillRolesViewData();

			return View();
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				await FillRolesViewData();
				return View(model);
			}

			if (!await _roleManager.RoleExistsAsync(model.Role))
			{
				await FillRolesViewData();
				ModelState.AddModelError(nameof(model.Role), "Vai trò không tồn tại.");
				return View(model);
			}

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				FName = model.FName,
				LName = model.LName,
				PhoneNumber = model.PhoneNumber
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.Role);
				return RedirectToAction(nameof(Index));
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			await FillRolesViewData();
			return View(model);
		}

		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			await FillRolesViewData();

			return View(await MapToViewModel(user));
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, UserViewModel model)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			if (id != model.Id)
				return BadRequest();

			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			if (!ModelState.IsValid)
			{
				await FillRolesViewData();
				return View(model);
			}

			if (!await _roleManager.RoleExistsAsync(model.Role))
			{
				await FillRolesViewData();
				ModelState.AddModelError(nameof(model.Role), "Vai trò không tồn tại.");
				return View(model);
			}

			var currentUser = await _userManager.GetUserAsync(User);
			var isSelf = currentUser is not null && currentUser.Id == id;
			var currentRoles = await _userManager.GetRolesAsync(user);

			user.FName = model.FName;
			user.LName = model.LName;
			user.Email = model.Email;
			user.PhoneNumber = model.PhoneNumber;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

				await FillRolesViewData();
				return View(model);
			}

			if (!currentRoles.Contains(model.Role))
			{
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				await _userManager.AddToRoleAsync(user, model.Role);
			}

			if (isSelf)
			{
				TempData["Message"] = "Bạn đã cập nhật thông tin của chính mình. Vui lòng đăng nhập lại để áp dụng vai trò mới.";
				await _signInManager.SignOutAsync();
				return RedirectToAction("SignIn", "Account");
			}

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			return View(await MapToViewModel(user));
		}

		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, UserViewModel model)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			if (id != model.Id)
				return BadRequest();

			var currentUser = await _userManager.GetUserAsync(User);

			if (currentUser is not null && currentUser.Id == id)
			{
				ModelState.AddModelError(string.Empty, "Không thể xoá chính tài khoản đang đăng nhập.");
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(id);

			if (user is null)
				return NotFound();

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
				return RedirectToAction(nameof(Index));

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}

		private async Task FillRolesViewData()
		{
			ViewBag.RolesList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
				_roleManager.Roles
					.Select(r => new { Value = r.Name, Text = DisplayNames.Role(r.Name) })
					.ToList(),
				"Value", "Text");
		}
    }
}
