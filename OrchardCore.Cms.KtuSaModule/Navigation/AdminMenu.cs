using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        builder
            .Add(T["Edit Content"],"1", content => content
                .AddClass("icon-class-fa-file-pen").AddClass("icon-class-fas")
                .Add(T["DUK"], dukContentType => dukContentType
                    .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = ContentTypeNames.Duk.ToString() })
                    .Permission(DukPermissions.ManageDuks)
                    .AddClass("icon-class-fa-circle-question").AddClass("icon-class-fas")

                ));

         return Task.CompletedTask;
    }
}