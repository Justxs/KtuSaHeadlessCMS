using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class StaticInfoMenu( 
    IStringLocalizer<AdminMenu> stringLocalizer,
    IHttpContextAccessor httpContextAccessor) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return ValueTask.CompletedTask;
        }

        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return ValueTask.CompletedTask;
        }

        builder.Add(T["Static info"], "2", content => content
            .AddClass("icon-class-fa-circle-info")
            .AddClass("icon-class-fas")
            .Add(T["Hero Section"], duk => duk
                .Action("List", "Admin", new
                {
                    area = "OrchardCore.Contents",
                    contentTypeId = HeroSection,
                })
                .Permission(HeroSectionPermissions.ManageHeroSections)
                .AddClass("icon-class-fa-list")
                .AddClass("icon-class-fas")
            )
        );

        return ValueTask.CompletedTask;
    }
}