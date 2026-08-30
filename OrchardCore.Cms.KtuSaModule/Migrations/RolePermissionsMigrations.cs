using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Data.Migration;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public sealed class RolePermissionsMigrations(RoleManager<IRole> roleManager) : DataMigration
{
    private const string LegacyAssignRoles = "AssignRoles";
    private const string ManageRoles = "ManageRoles";
    private const string ListContent = "ListContent";

    public async Task<int> CreateAsync()
    {
        await ReplacePermissionAsync(President, LegacyAssignRoles, ManageRoles);
        await AddPermissionAsync(CsaEditor, ListContent);
        await AddPermissionAsync(StatiusEditor, ContactPermissions.ManageStatiusContacts.Name);
        await AddPermissionAsync(StatiusEditor, EventPermissions.ManageStatiusEvents.Name);
        await AddPermissionAsync(StatiusEditor, SaUnitPermissions.ManageStatiusInfo.Name);

        return 1;
    }

    private async Task ReplacePermissionAsync(string roleName, string oldPermission, string newPermission)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null) return;

        var claims = await roleManager.GetClaimsAsync(role);
        foreach (var claim in claims.Where(claim =>
                     claim.Type == Permission.ClaimType && claim.Value == oldPermission))
            EnsureSucceeded(await roleManager.RemoveClaimAsync(role, claim), roleName, oldPermission);

        await AddPermissionAsync(role, roleName, newPermission);
    }

    private async Task AddPermissionAsync(string roleName, string permission)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null) return;

        await AddPermissionAsync(role, roleName, permission);
    }

    private async Task AddPermissionAsync(IRole role, string roleName, string permission)
    {
        var claims = await roleManager.GetClaimsAsync(role);
        if (claims.Any(claim => claim.Type == Permission.ClaimType && claim.Value == permission)) return;

        var claim = new Claim(Permission.ClaimType, permission);
        EnsureSucceeded(await roleManager.AddClaimAsync(role, claim), roleName, permission);
    }

    private static void EnsureSucceeded(IdentityResult result, string roleName, string permission)
    {
        if (result.Succeeded) return;

        var errors = string.Join("; ", result.Errors.Select(error => error.Description));
        throw new InvalidOperationException(
            $"Could not update permission '{permission}' for role '{roleName}': {errors}");
    }
}
