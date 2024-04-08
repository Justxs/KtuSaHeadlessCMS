using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class AdminMenu(
    IStringLocalizer<AdminMenu> stringLocalizer, 
    IAuthorizationService authorizationService, 
    IHttpContextAccessor httpContextAccessor) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public async Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return;
        }

        var eventPermissions = await new EventPermissions().GetPermissionsAsync();
        var hasEventPermission = await CheckIfUserHasPermissionAsync(eventPermissions, user);


        builder
            .Add(T["Need help content"],"1", content => content
                .AddClass("icon-class-fa-handshake-angle")
                .AddClass("icon-class-fas")
                .Add(T["FAQ"], dukContentType => dukContentType
                    .AddClass("icon-class-fa-circle-question")
                    .AddClass("icon-class-fas")
                    .Add(T["All FAQs"], duk => duk
                        .Action("List", "Admin", new
                        {
                            area = "OrchardCore.Contents",
                            contentTypeId = ContentTypeNames.Duk.ToString(),
                        })
                        .Permission(DukPermissions.ManageDuks)
                        .AddClass("icon-class-fa-list")
                        .AddClass("icon-class-fas")
                    )
                    .Add(T["Create new FAQ"], createAction => createAction
                        .Url($"/Admin/Contents/ContentTypes/{ContentTypeNames.Duk}/Create")
                        .Permission(DukPermissions.ManageDuks)
                        .AddClass("icon-class-fa-circle-plus")
                        .AddClass("icon-class-fas")
                    )
                )
            )
            .Add(T["For students Content"], "1", content => content
                .AddClass("icon-class-fa-graduation-cap")
                .AddClass("icon-class-fas")
            )
            .Add(T["About us Content"], "1", content => content
                .AddClass("icon-class-fa-circle-info")
                .AddClass("icon-class-fas")
            )
            .Add(T["Sponsors"], "1", content => content
                .AddClass("icon-class-fa-sack-dollar")
                .AddClass("icon-class-fas")
                .Add(T["All sponsors"], duk => duk
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = ContentTypeNames.Sponsor.ToString(),
                    })
                    .Permission(SponsorPermissions.ManageSponsors)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas")
                )
                .Add(T["Add new sponsor"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{ContentTypeNames.Sponsor}/Create")
                    .Permission(SponsorPermissions.ManageSponsors)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas")
                )
            )
            .Add(T["Articles"], "1", content => content
                .AddClass("icon-class-fa-newspaper")
                .AddClass("icon-class-fas")
                .Add(T["All articles"], eventContentType => eventContentType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = ContentTypeNames.Article.ToString(),
                    })
                    .Permission(ArticlePermissions.ManageArticles)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Create an article"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{ContentTypeNames.Article}/Create")
                    .Permission(ArticlePermissions.ManageArticles)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas"))
            )
            .Add(T["Contacts"], "1", content => content
                .AddClass("icon-class-fa-address-book")
                .AddClass("icon-class-fas")
                .Add(T["All current contacts"], eventContentType => eventContentType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = ContentTypeNames.Contact.ToString(),
                    })
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Create a new contact"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{ContentTypeNames.Contact}/Create")
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas"))
            );

        if (hasEventPermission)
        {
            builder.Add(T["Events"], "1", content => content
                .AddClass("icon-class-fa-calendar-days")
                .AddClass("icon-class-fas")
                .Add(T["All events"], eventContentType => eventContentType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents", 
                        contentTypeId = ContentTypeNames.Event.ToString(),
                    })
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Create an event"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{ContentTypeNames.Event}/Create")
                    .AddClass("icon-class-fa-calendar-plus")
                    .AddClass("icon-class-fas"))
            );
        }
    }

    private async Task<bool> CheckIfUserHasPermissionAsync(IEnumerable<Permission> permissions, ClaimsPrincipal user)
    {
        var hasEventPermission = false;
        foreach (var permission in permissions)
        {
            if (await authorizationService.AuthorizeAsync(user, permission))
            {
                hasEventPermission = true;
                break;
            }
        }

        return hasEventPermission;
    }
}