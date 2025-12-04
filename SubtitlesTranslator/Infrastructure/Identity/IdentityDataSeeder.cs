using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubtitlesTranslator.Application.Authorization;

namespace SubtitlesTranslator.Infrastructure.Identity
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var rolesWithPermissions = new Dictionary<string, string[]>
            {
                [RoleNames.Admin] = PermissionNames.All,
                [RoleNames.Support] = new[]
                {
                    PermissionNames.ViewAdminArea,
                    PermissionNames.GenerateInvoices
                }
            };

            foreach (var role in rolesWithPermissions.Keys)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                var identityRole = await roleManager.FindByNameAsync(role) ?? throw new InvalidOperationException($"Role {role} no longer exists after creation attempt.");
                var existingClaims = await roleManager.GetClaimsAsync(identityRole);
                var existingPermissionValues = existingClaims
                    .Where(c => c.Type == AuthorizationConstants.PermissionClaimType)
                    .Select(c => c.Value)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var permission in rolesWithPermissions[role])
                {
                    if (!existingPermissionValues.Contains(permission))
                    {
                        await roleManager.AddClaimAsync(identityRole, new Claim(AuthorizationConstants.PermissionClaimType, permission));
                    }
                }
            }

            var adminEmail = configuration["DefaultAdmin:Email"];
            var adminPassword = configuration["DefaultAdmin:Password"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(";", createResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to create default admin user: {errors}");
                    }
                }

                if (!await userManager.IsInRoleAsync(adminUser, RoleNames.Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
                }
            }
        }
    }
}
