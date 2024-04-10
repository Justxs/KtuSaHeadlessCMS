using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using System.Security.Claims;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class EventsMenu(
    IStringLocalizer<AdminMenu> stringLocalizer,
    IAuthorizationService authorizationService,
    IHttpContextAccessor httpContextAccessor)
    : INavigationProvider
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