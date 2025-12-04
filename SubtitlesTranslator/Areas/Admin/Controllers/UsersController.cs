using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubtitlesTranslator.Application.Authorization;
using SubtitlesTranslator.Areas.Admin.ViewModels;

namespace SubtitlesTranslator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = PermissionNames.ManageRoles)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string? searchTerm)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(u => (u.Email ?? "").Contains(term) || (u.UserName ?? "").Contains(term));
            }

            var users = await query
                .OrderBy(u => u.Email)
                .Select(u => new UserListItemViewModel
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    UserName = u.UserName,
                    IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow,
                    TwoFactorEnabled = u.TwoFactorEnabled
                })
                .ToListAsync();

            foreach (var user in users)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(identityUser);
                user.Roles = roles.ToArray();
            }

            var model = new UserIndexViewModel
            {
                SearchTerm = searchTerm,
                Users = users
            };

            return View(model);
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            var model = await BuildManageUserRolesViewModelAsync(id);
            return model == null ? NotFound() : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(ManageUserRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var reloadModel = await BuildManageUserRolesViewModelAsync(model.UserId);
                return View(reloadModel);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            var selectedRoles = model.Roles.Where(r => r.Assigned).Select(r => r.RoleName).ToList();
            var currentRoles = await _userManager.GetRolesAsync(user);

            var toRemove = currentRoles.Except(selectedRoles).ToList();
            var toAdd = selectedRoles.Except(currentRoles).ToList();

            if (toRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, toRemove);
                AddErrors(removeResult);
            }

            if (toAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, toAdd);
                AddErrors(addResult);
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                //foreach (var validator in _userManager.PasswordValidators) {
                //    var validation = await validator.ValidateAsync(_userManager, user, model.NewPassword);
                //    if (!validation.Succeeded) {
                //        AddErrors(validation);
                //        var reloadModel = await BuildManageUserRolesViewModelAsync(model.UserId);
                //        return View(reloadModel);
                //    }
                //}
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

                AddErrors(resetResult);
            }

            if (!ModelState.IsValid)
            {
                var reloadModel = await BuildManageUserRolesViewModelAsync(model.UserId);
                return View(reloadModel);
            }

            TempData["SuccessMsg"] = "User updated.";
            return RedirectToAction(nameof(EditRoles), new { id = model.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            TempData["SuccessMsg"] = "User locked.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, null);
            TempData["SuccessMsg"] = "User unlocked.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<ManageUserRolesViewModel?> BuildManageUserRolesViewModelAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var allRoles = await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleSelections = allRoles
                .Where(r => r != null)
                .Select(role => new RoleSelectionViewModel
                {
                    RoleName = role!,
                    Assigned = userRoles.Contains(role!)
                })
                .ToList();

            return new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roleSelections
            };
        }

        private void AddErrors(IdentityResult result)
        {
            if (result.Succeeded)
                return;

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
