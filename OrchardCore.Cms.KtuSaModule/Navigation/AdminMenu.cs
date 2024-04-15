using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class AdminMenu(
    IStringLocalizer<AdminMenu> stringLocalizer, 
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

        builder
            .Add(T["FAQ"], "2", dukContentType => dukContentType
                .AddClass("icon-class-fa-circle-question")
                .AddClass("icon-class-fas")
                .Add(T["All FAQs"], duk => duk
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = Duk,
                    })
                    .Permission(DukPermissions.ManageDuks)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas")
                )
                .Add(T["Create new FAQ"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{Duk}/Create")
                    .Permission(DukPermissions.ManageDuks)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas")
                )
            )
            .Add(T["Contacts"], "1", content => content
                .AddClass("icon-class-fa-address-book")
                .AddClass("icon-class-fas")
                .Add(T["All current contacts"], eventContentType => eventContentType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = Contact,
                    })
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Create a new contact"], createAction => createAction
                    .Url($"/Admin/Contents/ContentTypes/{Contact}/Create")
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas"))
            );

    }
}