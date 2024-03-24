using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class CardPartHandler : ContentPartHandler<CardPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, CardPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, CardPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }
}