using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Fields;

public class ImageUploadFieldDriver(IGoogleCloudService googleCloudService) : ContentFieldDisplayDriver<ImageUploadField>
{
    public override IDisplayResult Display(ImageUploadField field, BuildFieldDisplayContext context)
    {
        return Initialize<ImageUploadFieldViewModel>(
                GetDisplayShapeType(context),
                viewModel =>
                {
                    viewModel.FileId = field.FileId;
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                })
            .Location("Content");
    }

    public override IDisplayResult Edit(ImageUploadField field, BuildFieldEditorContext context)
    {
        return Initialize<ImageUploadFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.FileId = field.FileId;
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(ImageUploadField field, UpdateFieldEditorContext context)
    {
        var viewModel = new ImageUploadFieldViewModel();

        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        if (viewModel.UploadedFile is null && field.FileId is not null)
        {
            context.Updater.ModelState[$"{Prefix}.UploadedFile"]!.ValidationState = ModelValidationState.Valid;

            return await EditAsync(field, context);
        }

        if (viewModel.UploadedFile is not null)
        {
            var allowedContentTypes = new List<string> { "image/png", "image/jpeg" };
            if (!allowedContentTypes.Contains(viewModel.UploadedFile.ContentType.ToLowerInvariant()))
            {
                context.Updater.ModelState.AddModelError("ImageUploadField.UploadedFile", "Only PNG and JPEG files are allowed.");
            }
        }

        if (viewModel.UploadedFile is not null && viewModel.UploadedFile.Length != 0)
        {
            viewModel.FileId = field.FileId;
            field.FileId = await googleCloudService.UploadImageAsync(viewModel);
        }

        return await EditAsync(field, context);
    }
}