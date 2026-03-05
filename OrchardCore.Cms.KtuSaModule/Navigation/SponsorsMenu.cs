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
        if (!name.IsAdminMenu()) return ValueTask.CompletedTask;

        builder.Add(T["Sponsors"], "1", content => content
            .WithIcon("icon-class-fa-sack-dollar")
            .AddContentList(T["All sponsors"], Sponsor, SponsorPermissions.ManageSponsors)
            .AddCreateContentType(T["Add new sponsor"], Sponsor, SponsorPermissions.ManageSponsors)
        );

        return ValueTask.CompletedTask;
    }
}
