using Microsoft.AspNetCore.Identity;
using OrchardCore.Security;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class RoleSetupService(RoleManager<IRole> roleManager) : IRoleSetupService
{
    public async Task CreateCustomRoleAsync(string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new Role { RoleName = roleName };
            await roleManager.CreateAsync(role);
        }
    }
}