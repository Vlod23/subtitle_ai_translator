using System.Security.Claims;
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
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var viewModel = new List<RolePermissionsViewModel>();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var assignedPermissions = claims
                    .Where(c => c.Type == AuthorizationConstants.PermissionClaimType)
                    .Select(c => c.Value)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var permissionSelections = PermissionNames.All.Select(p => new PermissionSelectionViewModel
                {
                    Name = p,
                    DisplayName = GetPermissionDisplayName(p),
                    Assigned = assignedPermissions.Contains(p)
                }).ToList();

                viewModel.Add(new RolePermissionsViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name ?? string.Empty,
                    Permissions = permissionSelections
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RolePermissionsViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
                return NotFound();

            var selectedPermissions = model.Permissions.Where(p => p.Assigned).Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var permissionClaims = existingClaims.Where(c => c.Type == AuthorizationConstants.PermissionClaimType).ToList();

            foreach (var claim in permissionClaims)
            {
                if (!selectedPermissions.Contains(claim.Value))
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
            }

            var existingPermissionValues = permissionClaims.Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var permission in selectedPermissions)
            {
                if (!existingPermissionValues.Contains(permission))
                {
                    await _roleManager.AddClaimAsync(role, new Claim(AuthorizationConstants.PermissionClaimType, permission));
                }
            }

            TempData["SuccessMsg"] = "Role permissions updated.";
            return RedirectToAction(nameof(Index));
        }

        private static string GetPermissionDisplayName(string permission) => permission switch
        {
            PermissionNames.ManageRoles => "Manage users & roles",
            PermissionNames.GenerateInvoices => "Generate invoices",
            PermissionNames.DeleteSubtitles => "Delete subtitle translations",
            PermissionNames.ViewAdminArea => "Access admin area",
            _ => permission
        };
    }
}
