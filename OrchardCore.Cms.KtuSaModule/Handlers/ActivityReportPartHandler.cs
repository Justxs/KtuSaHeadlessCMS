using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ActivityReportPartHandler(IContentManager contentManager) : ContentPartHandler<ActivityReportPart>
{
    public override async Task UpdatedAsync(UpdateContentContext context, ActivityReportPart instance)
    {
        var saUnit = await contentManager.GetAsync(instance.SaUnit.ContentItemIds.First());
        var saUnitName = saUnit.As<SaUnitPart>().UnitName;

        context.ContentItem.DisplayText = $"{saUnitName} {instance.From:yyyy-MM-dd} - {instance.To:yyyy-MM-dd}";
    }

    public override async Task CreatedAsync(CreateContentContext context, ActivityReportPart instance)
    {
        var saUnit = await contentManager.GetAsync(instance.SaUnit.ContentItemIds.First());
        var saUnitName = saUnit.As<SaUnitPart>().UnitName;

        context.ContentItem.DisplayText = $"{saUnitName} {instance.From:yyyy-MM-dd} - {instance.To:yyyy-MM-dd}";

    }
}