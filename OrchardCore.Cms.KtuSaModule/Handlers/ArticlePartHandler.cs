using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ArticlePartHandler : ContentPartHandler<ArticlePart>
{
    public override Task UpdatedAsync(UpdateContentContext context, ArticlePart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, ArticlePart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }
}