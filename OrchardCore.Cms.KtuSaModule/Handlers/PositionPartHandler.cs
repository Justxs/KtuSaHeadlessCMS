using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class PositionPartHandler : ContentPartHandler<PositionPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, PositionPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.NameLt} / {instance.NameEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, PositionPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.NameLt} / {instance.NameEn}";

        return Task.CompletedTask;
    }
}