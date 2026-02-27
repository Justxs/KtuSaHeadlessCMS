using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class ContactPartDriver : ContentPartDisplayDriver<ContactPart>
{
    public override IDisplayResult Display(ContactPart part, BuildPartDisplayContext context)
    {
        return Initialize<ContactPartViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    model.PhoneNumber = part.PhoneNumber;
                    model.Email = part.Email;
                })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(ContactPart part, BuildPartEditorContext context)
    {
        return Initialize<ContactPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.PhoneNumber = part.PhoneNumber;
                model.Email = part.Email;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(ContactPart part, UpdatePartEditorContext context)
    {
        var model = new ContactPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        part.PhoneNumber = model.PhoneNumber;
        part.Email = model.Email;

        return Edit(part, context);
    }
}