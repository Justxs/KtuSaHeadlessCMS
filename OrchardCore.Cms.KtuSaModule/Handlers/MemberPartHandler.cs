using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class MemberPartHandler : ContentPartHandler<MemberPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, MemberPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.Name}";

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, MemberPart instance)
    {
        context.ContentItem.DisplayText = $"{instance.Name}";

        return Task.CompletedTask;
    }
}