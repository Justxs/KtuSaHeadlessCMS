using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.Environment.Shell.Removing;
using OrchardCore.Modules;

namespace OrchardCore.Cms.KtuSaModule.Events;

public class StartupConfigurationHandler(IRoleSetupService roleSetupService) : IModularTenantEvents
{
    public async Task ActivatingAsync()
    {
        foreach (var role in Enum.GetNames(typeof(Models.Enums.Roles)))
        {
            await roleSetupService.CreateCustomRoleAsync(role);
        }
    }

    public Task ActivatedAsync()
    {
        return Task.CompletedTask;
    }

    public Task TerminatingAsync()
    {
        return Task.CompletedTask;
    }

    public Task TerminatedAsync()
    {
        return Task.CompletedTask;
    }

    public Task RemovingAsync(ShellRemovingContext context)
    {
        return Task.CompletedTask;
    }
}