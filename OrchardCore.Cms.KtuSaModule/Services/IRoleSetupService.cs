namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IRoleSetupService
{
    Task CreateCustomRoleAsync(string roleName);
}