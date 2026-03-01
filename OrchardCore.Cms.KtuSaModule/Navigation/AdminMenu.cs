using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class AdminMenu(
    IStringLocalizer<AdminMenu> stringLocalizer,
    IRepository repository) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public async ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return;

        var faqPages = await repository.GetAllAsync(FaqPage);
        var faqPage = faqPages.FirstOrDefault();

        if (faqPage is not null)
        {
            builder.Add(T["FAQ"], "2", faq => faq
                .Url($"/Admin/Contents/ContentItems/{faqPage.ContentItemId}/Display")
                .Permission(FaqPermissions.ManageFaqs)
                .AddClass("icon-class-fa-circle-question")
                .AddClass("icon-class-fas")
            );
        }

        builder
            .Add(T["Contacts"], "1", content => content
                .AddClass("icon-class-fa-address-book")
                .AddClass("icon-class-fas")
                .Add(T["All contacts"], contactsType => contactsType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = Contact
                    })
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Main contacts"], mainContacts => mainContacts
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = MainContact
                    })
                    .Permission(ContactPermissions.ManageCsaContacts)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["All positions"], positionsType => positionsType
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = Position
                    })
                    .Permission(ContactPermissions.ManagePositions)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
            )
            .Add(T["Activity Reports"], "3", activityReports => activityReports
                .AddClass("icon-class-fa-clipboard-list")
                .AddClass("icon-class-fas")
                .Add(T["All activity reports"], list => list
                    .Action("List", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentTypeId = ActivityReport
                    })
                    .Permission(ActivityReportPermissions.ManageActivityReports)
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"))
                .Add(T["Create activity report"], create => create
                    .Url($"/Admin/Contents/ContentTypes/{ActivityReport}/Create")
                    .Permission(ActivityReportPermissions.ManageActivityReports)
                    .AddClass("icon-class-fa-circle-plus")
                    .AddClass("icon-class-fas"))
            );
    }
}