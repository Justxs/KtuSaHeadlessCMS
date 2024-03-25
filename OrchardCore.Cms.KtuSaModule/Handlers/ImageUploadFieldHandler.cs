using OrchardCore.Cms.KtuSaModule.Models.FIelds;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class ImageUploadFieldHandler(IGoogleCloudService googleCloudService) : ContentFieldHandler<ImageUploadField>
{
    public override async Task RemovedAsync(RemoveContentFieldContext context, ImageUploadField field)
    {
        await googleCloudService.RemoveFileAsync(field.FileId);
    }
}