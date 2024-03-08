using Microsoft.AspNetCore.Http;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class ImageUploadFieldViewModel
{
    public string FileId { get; set; } = null!;
    public IFormFile UploadedFile { get; set; } = null!;
}