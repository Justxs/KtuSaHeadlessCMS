using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Fields;

public class PdfUploadFieldDriver(IGoogleCloudService googleCloudService) : ContentFieldDisplayDriver<PdfUploadField>
{
    public override IDisplayResult Edit(PdfUploadField field, BuildFieldEditorContext context)
    {
        return Initialize<PdfUploadFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.FileId = field.FileId;
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(PdfUploadField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new PdfUploadFieldViewModel();

        await updater.TryUpdateModelAsync(viewModel, Prefix);

        if (viewModel.UploadedFile is null && field.FileId is not null)
        {
            updater.ModelState[$"{Prefix}.UploadedFile"]!.ValidationState = ModelValidationState.Valid;

            return await EditAsync(field, context);
        }

        if (viewModel.UploadedFile is not null)
        {
            var allowedContentTypes = new List<string> { "application/pdf" };
            if (!allowedContentTypes.Contains(viewModel.UploadedFile.ContentType.ToLowerInvariant()))
            {
                updater.ModelState.AddModelError("ImageUploadField.UploadedFile", "Only PNG and JPEG files are allowed.");
            }
        }

        if (viewModel.UploadedFile is not null && viewModel.UploadedFile.Length != 0)
        {
            viewModel.FileId = field.FileId;
            field.FileId = await googleCloudService.UploadPdfAsync(viewModel);
        }

        return await EditAsync(field, context);
    }
}