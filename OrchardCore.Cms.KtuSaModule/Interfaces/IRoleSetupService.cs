namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IRoleSetupService
{
    Task CreateCustomRoleAsync(string roleName);
}