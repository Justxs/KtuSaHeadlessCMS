using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Fields;

public class PdfUploadField : ContentField
{
    public string FileId { get; set; } = null!;
}