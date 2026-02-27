namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class ArticlePreviewResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("Article title in the requested language")]
    public required string Title { get; set; }

    [Description("Plain-text preview extracted from the article body")]
    public required string Preview { get; set; }

    [Description("Date and time the article was created (UTC)")]
    public DateTime CreatedDate { get; set; }

    [Description("File ID of the article thumbnail image")]
    public required string ThumbnailImageId { get; set; }
}