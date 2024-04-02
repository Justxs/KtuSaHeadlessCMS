using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class EventPartHandler : ContentPartHandler<EventPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, EventPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, EventPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }
}