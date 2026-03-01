using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class SponsorsMenu(IStringLocalizer<SponsorsMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return ValueTask.CompletedTask;

        builder.Add(T["Sponsors"], "1", content => content
            .AddClass("icon-class-fa-sack-dollar")
            .AddClass("icon-class-fas")
            .Add(T["All sponsors"], duk => duk
                .Action("List", "Admin", new
                {
                    area = "OrchardCore.Contents",
                    contentTypeId = Sponsor
                })
                .Permission(SponsorPermissions.ManageSponsors)
                .AddClass("icon-class-fa-list")
                .AddClass("icon-class-fas")
            )
            .Add(T["Add new sponsor"], createAction => createAction
                .Url($"/Admin/Contents/ContentTypes/{Sponsor}/Create")
                .Permission(SponsorPermissions.ManageSponsors)
                .AddClass("icon-class-fa-circle-plus")
                .AddClass("icon-class-fas")
            )
        );

        return ValueTask.CompletedTask;
    }
}