using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class PositionPartDriver : ContentPartDisplayDriver<PositionPart>
{
    public override IDisplayResult Display(PositionPart part, BuildPartDisplayContext context)
    {
        return Initialize<PositionPartViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    model.NameLt = part.NameLt;
                    model.DescriptionLt = part.DescriptionLt;
                    model.NameEn = part.NameEn;
                    model.DescriptionEn = part.DescriptionEn;
                })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(PositionPart part, BuildPartEditorContext context)
    {
        return Initialize<PositionPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.NameLt = part.NameLt;
                model.DescriptionLt = part.DescriptionLt;
                model.NameEn = part.NameEn;
                model.DescriptionEn = part.DescriptionEn;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(PositionPart part, UpdatePartEditorContext context)
    {
        var model = new PositionPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        part.NameLt = model.NameLt;
        part.DescriptionLt = model.DescriptionLt;
        part.NameEn = model.NameEn;
        part.DescriptionEn = model.DescriptionEn;

        return Edit(part, context);
    }
}