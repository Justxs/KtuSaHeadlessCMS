using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class AddressPartDriver : ContentPartDisplayDriver<AddressPart>
{
    public override IDisplayResult Display(AddressPart part, BuildPartDisplayContext context)
    {
        return Initialize<AddressPartViewModel>(
                GetDisplayShapeType(context), model => { model.Address = part.Address; })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(AddressPart part, BuildPartEditorContext context)
    {
        return Initialize<AddressPartViewModel>(
            GetEditorShapeType(context), model => { model.Address = part.Address; });
    }

    public override async Task<IDisplayResult> UpdateAsync(AddressPart part, UpdatePartEditorContext context)
    {
        var model = new AddressPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return Edit(part, context);

        part.Address = model.Address;

        return Edit(part, context);
    }
}