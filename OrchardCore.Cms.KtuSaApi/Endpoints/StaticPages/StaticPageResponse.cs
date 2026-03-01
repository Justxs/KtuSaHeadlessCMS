namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class StaticPageResponse
{
    [Description("Title in the requested language")]
    public required string Title { get; set; }

    [Description("Hero section description text in the requested language")]
    public string? Description { get; set; }

    [Description("File ID of the hero section background image")]
    public string? ImgSrc { get; set; }

    [Description("HTML body of the static page in the requested language")]
    public string? Body { get; set; }
}