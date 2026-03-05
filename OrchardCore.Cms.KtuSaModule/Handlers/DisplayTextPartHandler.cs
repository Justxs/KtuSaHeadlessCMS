using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public abstract class DisplayTextPartHandler<TPart> : ContentPartHandler<TPart>
    where TPart : ContentPart, new()
{
    protected abstract string GetDisplayText(TPart part);

    public override Task UpdatedAsync(UpdateContentContext context, TPart instance)
    {
        context.ContentItem.DisplayText = GetDisplayText(instance);
        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, TPart instance)
    {
        context.ContentItem.DisplayText = GetDisplayText(instance);
        return Task.CompletedTask;
    }
}