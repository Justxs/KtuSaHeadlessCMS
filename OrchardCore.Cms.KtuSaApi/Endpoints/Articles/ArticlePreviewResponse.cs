namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class ArticlePreviewResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Article title in the requested language.")]
    public required string Title { get; set; }

    [Description("Plain-text preview extracted from the article body.")]
    public required string Preview { get; set; }

    [Description("Article creation timestamp in UTC.")]
    public DateTime CreatedDate { get; set; }

    [Description("Public URL of the article thumbnail image. Can be empty when no image is configured.")]
    public required string ThumbnailImageUrl { get; set; }
}
