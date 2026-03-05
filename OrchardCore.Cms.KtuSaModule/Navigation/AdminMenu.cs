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
        if (!name.IsAdminMenu()) return;

        var faqPages = await repository.GetAllAsync(FaqPage);
        var faqPage = faqPages.FirstOrDefault();

        if (faqPage is not null)
            builder.Add(T["FAQ"], "2", faq => faq
                .Url($"/Admin/Contents/ContentItems/{faqPage.ContentItemId}/Display")
                .Permission(FaqPermissions.ManageFaqs)
                .WithIcon("icon-class-fa-circle-question")
            );

        builder
            .Add(T["Contacts"], "1", content => content
                .WithIcon("icon-class-fa-address-book")
                .AddContentList(T["All contacts"], Contact, ContactPermissions.ManageCsaContacts)
                .AddContentList(T["Main contacts"], MainContact, ContactPermissions.ManageCsaContacts)
                .AddContentList(T["All positions"], Position, ContactPermissions.ManagePositions)
            )
            .Add(T["Activity Reports"], "3", activityReports => activityReports
                .WithIcon("icon-class-fa-clipboard-list")
                .AddContentList(
                    T["All activity reports"],
                    ActivityReport,
                    ActivityReportPermissions.ManageActivityReports)
                .AddCreateContentType(
                    T["Create activity report"],
                    ActivityReport,
                    ActivityReportPermissions.ManageActivityReports)
            );
    }
}
