using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class StaticPagePartDriver : ContentPartDisplayDriver<StaticPagePart>
{
    public override IDisplayResult Edit(StaticPagePart part, BuildPartEditorContext context)
    {
        return Initialize<StaticPagePartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.TitleLt = part.TitleLt;
                model.TitleEn = part.TitleEn;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(StaticPagePart part, UpdatePartEditorContext context)
    {
        var model = new StaticPagePartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        part.TitleLt = model.TitleLt;
        part.TitleEn = model.TitleEn;

        return Edit(part, context);
    }
}