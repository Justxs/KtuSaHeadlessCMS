using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class ArticleContentResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Article title in the requested language.")]
    public required string Title { get; set; }

    [Description("Structured content blocks of the article body in the requested language.")]
    public required List<ContentBlockResponse> Blocks { get; set; }

    [Description("Estimated reading time generated from article text (for example: '1 min.' or '3 min.').")]
    public string? ReadingTime { get; set; }

    [Description("Article creation timestamp in UTC.")]
    public DateTime CreatedDate { get; set; }

    [Description("Public URL of the article thumbnail image. Can be empty when no image is configured.")]
    public required string ThumbnailImageUrl { get; set; }

    [Description("Section headings extracted from paragraph HTML (h1-h6 tags), suitable for a table of contents.")]
    public List<string>? ContentList { get; set; }
}
