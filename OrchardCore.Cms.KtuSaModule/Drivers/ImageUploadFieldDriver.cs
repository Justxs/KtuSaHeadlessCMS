using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class ImageUploadFieldDriver(IGoogleDriveService googleDriveService) : ContentFieldDisplayDriver<ImageUploadField>
{
    public override IDisplayResult Display(ImageUploadField field, BuildFieldDisplayContext context) =>
        Initialize<ImageUploadFieldViewModel>(
            GetDisplayShapeType(context), 
            viewModel =>
            {
                viewModel.FileId = field.FileId;
            })
        .Location("Content");

    public override IDisplayResult Edit(ImageUploadField field, BuildFieldEditorContext context) =>
        Initialize<ImageUploadFieldViewModel>(
            GetEditorShapeType(context), 
            viewModel =>
            {
                viewModel.FileId = field.FileId;
            })
        .Location("Content");

    public override async Task<IDisplayResult> UpdateAsync(ImageUploadField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new ImageUploadFieldViewModel();
        
        if (await updater.TryUpdateModelAsync(viewModel, "ArticlePart.ImageUploadField"))
        {
            field.FileId = await googleDriveService.UploadImageAsync(viewModel);
        }

        return Edit(field, context);
    }
}