using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ActivityReportPartHandler(IContentManager contentManager) : ContentPartHandler<ActivityReportPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, ActivityReportPart instance)
    {
        return SetDisplayTextAsync(context.ContentItem, instance);
    }

    public override Task CreatedAsync(CreateContentContext context, ActivityReportPart instance)
    {
        return SetDisplayTextAsync(context.ContentItem, instance);
    }

    private async Task SetDisplayTextAsync(ContentItem contentItem, ActivityReportPart instance)
    {
        var saUnit = await contentManager.GetAsync(instance.SaUnit.ContentItemIds.First());
        var saUnitName = saUnit.As<SaUnitPart>().UnitName;

        contentItem.DisplayText = $"{saUnitName} {instance.From:yyyy-MM-dd} - {instance.To:yyyy-MM-dd}";
    }
}