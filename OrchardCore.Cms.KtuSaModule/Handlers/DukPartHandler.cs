using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class DukPartHandler : ContentPartHandler<DukPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, DukPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.QuestionLt} / {instance.QuestionEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, DukPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.QuestionLt} / {instance.QuestionEn}";

        return Task.CompletedTask;
    }
}