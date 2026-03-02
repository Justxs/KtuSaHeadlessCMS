using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Flows.Models;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ContentFlowInitializationHandler(IServiceProvider serviceProvider) : ContentHandlerBase
{
    private static readonly string[] FlowContentTypes = [Article, Event, StaticPage];

    public override async Task InitializingAsync(InitializingContentContext context)
    {
        if (!FlowContentTypes.Contains(context.ContentItem.ContentType))
            return;

        var contentManager = serviceProvider.GetRequiredService<IContentManager>();

        foreach (var flowName in (string[])["ContentLt", "ContentEn"])
        {
            var flow = context.ContentItem.Get<FlowPart>(flowName);

            if (flow is null || flow.Widgets is not { Count: 0 })
                continue;

            var paragraph = await contentManager.NewAsync(ParagraphWidget);
            paragraph.Weld<FlowMetadata>();

            flow.Widgets.Add(paragraph);

            context.ContentItem.Apply(flowName, flow);
        }
    }
}
