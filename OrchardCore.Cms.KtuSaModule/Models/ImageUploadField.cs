using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class ImageUploadField : ContentField
{
    public string FileId { get; set; } = null!;
}