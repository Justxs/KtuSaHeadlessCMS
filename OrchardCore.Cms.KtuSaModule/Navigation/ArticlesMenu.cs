using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class ArticlesMenu(IStringLocalizer<ArticlesMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return ValueTask.CompletedTask;

        builder.Add(T["Articles"], content => content
            .AddClass("icon-class-fa-newspaper")
            .AddClass("icon-class-fas")
            .Add(T["All articles"], eventContentType => eventContentType
                .Action("List", "Admin", new
                {
                    area = "OrchardCore.Contents",
                    contentTypeId = Article
                })
                .Permission(ArticlePermissions.ManageArticles)
                .AddClass("icon-class-fa-list")
                .AddClass("icon-class-fas"))
            .Add(T["Create an article"], createAction => createAction
                .Url($"/Admin/Contents/ContentTypes/{Article}/Create")
                .Permission(ArticlePermissions.ManageArticles)
                .AddClass("icon-class-fa-circle-plus")
                .AddClass("icon-class-fas"))
        );
        return ValueTask.CompletedTask;
    }
}