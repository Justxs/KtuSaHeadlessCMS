using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class SaUnitPartDriver : ContentPartDisplayDriver<SaUnitPart>
{
    public override IDisplayResult Display(SaUnitPart part, BuildPartDisplayContext context)
    {
        return Initialize<SaUnitPartViewModel>(
            GetDisplayShapeType(context), model =>
            {
                model.UnitName = part.UnitName;
                model.DescriptionLt = part.DescriptionLt;
                model.DescriptionEn = part.DescriptionEn;
            })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(SaUnitPart part, BuildPartEditorContext context)
    {
        return Initialize<SaUnitPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.UnitName = part.UnitName;
                model.DescriptionLt = part.DescriptionLt;
                model.DescriptionEn = part.DescriptionEn;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(SaUnitPart part, UpdatePartEditorContext context)
    {
        var model = new SaUnitPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        part.UnitName = model.UnitName;
        part.DescriptionLt = model.DescriptionLt;
        part.DescriptionEn = model.DescriptionEn;


        return Edit(part, context);
    }
}