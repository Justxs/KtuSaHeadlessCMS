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
                    model.NameLT = part.NameLt;
                    model.DescriptionLT = part.DescriptionLt;
                    model.NameEN = part.NameEn;
                    model.DescriptionEN = part.DescriptionEn;
                })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(PositionPart part, BuildPartEditorContext context)
    {
        return Initialize<PositionPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.NameLT = part.NameLt;
                model.DescriptionLT = part.DescriptionLt;
                model.NameEN = part.NameEn;
                model.DescriptionEN = part.DescriptionEn;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(PositionPart part, UpdatePartEditorContext context)
    {
        var model = new PositionPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        part.NameLt = model.NameLT;
        part.DescriptionLt = model.DescriptionLT;
        part.NameEn = model.NameEN;
        part.DescriptionEn = model.DescriptionEN;

        return Edit(part, context);
    }
}