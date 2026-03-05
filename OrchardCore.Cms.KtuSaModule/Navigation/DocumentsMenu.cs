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
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return ValueTask.CompletedTask;

        builder.Add(T["Documents"], "2", content => content
            .AddClass("icon-class-fa-file-lines")
            .AddClass("icon-class-fas")
            .Add(T["All Document categories"], eventContentType => eventContentType
                .Action("List", "Admin", new
                {
                    area = "OrchardCore.Contents",
                    contentTypeId = DocumentCategory
                })
                .Permission(DocumentsPermissions.ManageDocuments)
                .AddClass("icon-class-fa-list")
                .AddClass("icon-class-fas"))
            .Add(T["Create new document category"], createAction => createAction
                .Url($"/Admin/Contents/ContentTypes/{DocumentCategory}/Create")
                .Permission(DocumentsPermissions.ManageDocuments)
                .AddClass("icon-class-fa-circle-plus")
                .AddClass("icon-class-fas"))
        );
        return ValueTask.CompletedTask;
    }
}