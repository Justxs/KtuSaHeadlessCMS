using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class PositionPartDriver : ContentPartDisplayDriver<PositionPart>
{
    public override IDisplayResult Display(PositionPart part, BuildPartDisplayContext context)
    {
        return Initialize<PositionPartViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    model.Name = part.Name;
                    model.Description = part.Description;
                })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(PositionPart part, BuildPartEditorContext context)
    {
        return Initialize<PositionPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.Name = part.Name;
                model.Description = part.Description;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(PositionPart part, UpdatePartEditorContext context)
    {
        var model = new PositionPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        part.Name = model.Name;
        part.Description = model.Description;

        return Edit(part, context);
    }
}