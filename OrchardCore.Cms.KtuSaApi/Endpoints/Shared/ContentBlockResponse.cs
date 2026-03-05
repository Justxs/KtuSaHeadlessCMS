namespace OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

public class ContentBlockResponse
{
    [Description("Block type. Possible values: 'paragraph', 'image', 'video', 'pdf', or 'carousel'.")]
    public required string Type { get; set; }

    [Description("HTML content (only for 'paragraph' blocks).")]
    public string? Html { get; set; }

    [Description("Public image URL (only for 'image' blocks).")]
    public string? ImageUrl { get; set; }

    [Description("Video URL (only for 'video' blocks).")]
    public string? VideoUrl { get; set; }

    [Description("Public PDF document URL (only for 'pdf' blocks).")]
    public string? PdfUrl { get; set; }

    [Description("List of public image URLs (only for 'carousel' blocks).")]
    public List<string>? ImageUrls { get; set; }
}
