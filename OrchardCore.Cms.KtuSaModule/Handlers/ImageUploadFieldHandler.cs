using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ImageUploadFieldHandler(IGoogleDriveService googleDriveService) : ContentFieldHandler<ImageUploadField>
{
    public override async Task RemovedAsync(RemoveContentFieldContext context, ImageUploadField field)
    {
        await googleDriveService.RemoveFileAsync(field.FileId);
    }
}