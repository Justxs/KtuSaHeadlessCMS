using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public override IDisplayResult Display(ImageUploadField field, BuildFieldDisplayContext context)
    {
        return Initialize<ImageUploadFieldViewModel>(
                GetDisplayShapeType(context),
                viewModel => { viewModel.FileId = field.FileId; })
            .Location("Content");
    }

    public override IDisplayResult Edit(ImageUploadField field, BuildFieldEditorContext context)
    {
        return Initialize<ImageUploadFieldViewModel>(
                GetEditorShapeType(context),
                viewModel => { viewModel.FileId = field.FileId; })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(ImageUploadField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new ImageUploadFieldViewModel();

        await updater.TryUpdateModelAsync(viewModel, "ImageUploadField");

        if (viewModel.UploadedFile is null && field.FileId is not null)
        {
            updater.ModelState["ImageUploadField.UploadedFile"]!.ValidationState = ModelValidationState.Valid;
            return await EditAsync(field, context);
        }

        if (viewModel.UploadedFile is not null)
        {
            var allowedContentTypes = new List<string> { "image/png", "image/jpeg" };
            if (!allowedContentTypes.Contains(viewModel.UploadedFile.ContentType.ToLowerInvariant()))
            {
                updater.ModelState.AddModelError("ImageUploadField.UploadedFile", "Only PNG and JPEG files are allowed.");
            }
        }

        if (viewModel.UploadedFile.Length != 0)
        {
            viewModel.FileId = field.FileId;
            field.FileId = await googleDriveService.UploadImageAsync(viewModel);
        }

        return await EditAsync(field, context);
    }
}