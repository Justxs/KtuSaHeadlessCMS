using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Fields;

public class ImageUploadField : ContentField
{
    public string FileId { get; set; } = null!;
}