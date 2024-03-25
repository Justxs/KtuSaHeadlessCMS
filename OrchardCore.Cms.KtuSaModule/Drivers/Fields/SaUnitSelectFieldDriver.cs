using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Cms.KtuSaModule.Models.FIelds;
using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Fields;

public class SaUnitSelectFieldDriver : ContentFieldDisplayDriver<SaUnitSelectField>
{
    public override IDisplayResult Display(SaUnitSelectField field, BuildFieldDisplayContext context)
    {
        return Initialize<SaUnitSelectFieldViewModel>(
                GetDisplayShapeType(context),
                viewModel =>
                {
                    viewModel.SaUnit = field.SaUnit;
                })
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(SaUnitSelectField field, BuildFieldEditorContext context)
    {
        return Initialize<SaUnitSelectFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.SaUnit = field.SaUnit;
                })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(SaUnitSelectField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new SaUnitSelectFieldViewModel();

        await updater.TryUpdateModelAsync(viewModel, Prefix);

        field.SaUnit = viewModel.SaUnit;

        return await EditAsync(field, context);
    }
}