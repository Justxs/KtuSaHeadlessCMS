namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class ArticleContentResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("Article title in the requested language")]
    public required string Title { get; set; }

    [Description("Full HTML body of the article in the requested language")]
    public required string HtmlBody { get; set; }

    [Description("Estimated reading time (e.g. '3 min read')")]
    public string? ReadingTime { get; set; }

    [Description("Date and time the article was created (UTC)")]
    public DateTime CreatedDate { get; set; }

    [Description("File ID of the article thumbnail image")]
    public required string ThumbnailImageId { get; set; }

    [Description("Section headings extracted from the HTML body for a table of contents")]
    public List<string>? ContentList { get; set; }
}