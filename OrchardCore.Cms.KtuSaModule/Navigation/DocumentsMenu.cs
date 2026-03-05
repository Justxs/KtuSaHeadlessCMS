using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class DocumentsMenu(IStringLocalizer<DocumentsMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!name.IsAdminMenu()) return ValueTask.CompletedTask;

        builder.Add(T["Documents"], "2", content => content
            .WithIcon("icon-class-fa-file-lines")
            .AddContentList(T["All Document categories"], DocumentCategory, DocumentsPermissions.ManageDocuments)
            .AddCreateContentType(
                T["Create new document category"],
                DocumentCategory,
                DocumentsPermissions.ManageDocuments)
        );

        return ValueTask.CompletedTask;
    }
}
