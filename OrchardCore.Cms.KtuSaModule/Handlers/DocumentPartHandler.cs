using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class DocumentPartHandler : ContentPartHandler<DocumentPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, DocumentPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, DocumentPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.TitleLt} / {instance.TitleEn}";

        return Task.CompletedTask;
    }
}