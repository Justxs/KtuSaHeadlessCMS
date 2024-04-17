using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement.Handlers;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class PdfUploadFieldHandler(IGoogleCloudService googleCloudService) : ContentFieldHandler<PdfUploadField>
{
    public override async Task RemovedAsync(RemoveContentFieldContext context, PdfUploadField field)
    {
        await googleCloudService.RemoveFileAsync(field.FileId);
    }
}