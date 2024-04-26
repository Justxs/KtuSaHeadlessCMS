using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class MemberPartDriver(IContentManager contentManager) : ContentPartDisplayDriver<MemberPart>
{
    public override async Task<IDisplayResult> DisplayAsync(MemberPart part, BuildPartDisplayContext context)
    {
        var saUnitField = await contentManager.GetAsync(part.SaUnit.ContentItemIds);

        return Initialize<MemberPartViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    if (saUnitField != null)
                    {
                        model.SaUnit = saUnitField.Select(organiser => organiser.DisplayText).FirstOrDefault();
                    }
                })
                .Location("SummaryAdmin", "Tags:11");
    }

    public override IDisplayResult Edit(MemberPart part, BuildPartEditorContext context)
    {
        return Initialize<MemberPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.Name = part.Name;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(MemberPart part, UpdatePartEditorContext context)
    {
        var model = new MemberPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        part.Name = model.Name;

        return Edit(part, context);
    }
}