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
                    if (saUnitField == null)
                        model.SaUnit = saUnitField.Select(organiser => organiser.DisplayText).FirstOrDefault();

                    model.Email = part.Email;
                    model.Index = part.Index;
                })
            .Location("SummaryAdmin", "Tags:11");
    }

    public override IDisplayResult Edit(MemberPart part, BuildPartEditorContext context)
    {
        return Initialize<MemberPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.Name = part.Name;
                model.Email = part.Email;
                model.Index = part.Index;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(MemberPart part, UpdatePartEditorContext context)
    {
        var model = new MemberPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        part.Name = model.Name;
        part.Email = model.Email;
        part.Index = model.Index;

        return Edit(part, context);
    }
}