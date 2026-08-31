using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class BackupMenu(IStringLocalizer<BackupMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!name.IsAdminMenu()) return ValueTask.CompletedTask;

        builder.Add(T["Backup"], "9", backup => backup
            .WithIcon("icon-class-fa-database")
            .Add(T["Database backup"], item => item
                .Url("/Admin/Backup")
                .Permission(BackupPermissions.ManageBackups)
                .WithIcon("icon-class-fa-download"))
        );

        return ValueTask.CompletedTask;
    }
}
