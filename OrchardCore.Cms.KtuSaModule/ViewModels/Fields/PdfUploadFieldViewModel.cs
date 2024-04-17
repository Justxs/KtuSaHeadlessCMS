using Microsoft.AspNetCore.Http;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Fields;

public class PdfUploadFieldViewModel
{
    public string? Label { get; set; }

    public string? FileId { get; set; }

    public IFormFile UploadedFile { get; set; } = null!;
}