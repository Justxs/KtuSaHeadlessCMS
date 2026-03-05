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
        if (!name.IsAdminMenu()) return ValueTask.CompletedTask;

        builder.Add(T["Articles"], content => content
            .WithIcon("icon-class-fa-newspaper")
            .AddContentList(T["All articles"], Article, ArticlePermissions.ManageArticles)
            .AddCreateContentType(T["Create an article"], Article, ArticlePermissions.ManageArticles)
        );

        return ValueTask.CompletedTask;
    }
}
