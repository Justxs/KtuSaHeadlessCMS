using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class ImageUploadFieldDriver : ContentFieldDisplayDriver<ImageUploadField>
{
    public override IDisplayResult Display(ImageUploadField field, BuildFieldDisplayContext context)
    {
        return Initialize<ImageUploadFieldViewModel>("ImageUploadField", model =>
            {
                model.FileId = field.FileId;
            })
            .Location("Content");
    }

    public override IDisplayResult Edit(ImageUploadField field, BuildFieldEditorContext context)
    {
        return Initialize<ImageUploadFieldViewModel>("ImageUploadField_Edit", model =>
        {
            model.FileId = field.FileId;
        });
    }

    public override async Task<IDisplayResult> UpdateAsync(ImageUploadField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new ImageUploadFieldViewModel();

        if (await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            field.FileId = viewModel.FileId;
        }

        return Edit(field, context);
    }
}