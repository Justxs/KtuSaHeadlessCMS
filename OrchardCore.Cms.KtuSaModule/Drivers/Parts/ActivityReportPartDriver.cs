using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class ActivityReportPartDriver(IResourceManager resourceManager) : ContentPartDisplayDriver<ActivityReportPart>
{
    public override IDisplayResult Edit(ActivityReportPart part, BuildPartEditorContext context)
    {
        var settings = resourceManager.RegisterResource("script", "FlatpickrJs");
        settings.AtHead();

        settings = resourceManager.RegisterResource("stylesheet", "FlatpickrCss");
        settings.AtHead();

        return Initialize<ActivityReportPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.From = part.From;
                model.To = part.To;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(ActivityReportPart part, UpdatePartEditorContext context)
    {
        var model = new ActivityReportPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        if (model.To < model.From)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".To",
                "The activity report to date must be later than from date");

            return await EditAsync(part, context);
        }

        part.From = model.From;
        part.To = model.To;

        return Edit(part, context);
    }
}