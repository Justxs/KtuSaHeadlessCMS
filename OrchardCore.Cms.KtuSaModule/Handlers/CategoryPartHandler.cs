using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class CategoryPartHandler : ContentPartHandler<CategoryPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, CategoryPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, CategoryPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }
}