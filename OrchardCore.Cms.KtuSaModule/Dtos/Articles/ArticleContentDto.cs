namespace OrchardCore.Cms.KtuSaModule.Dtos.Articles;

public class ArticleContentDto
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string HtmlBody { get; set; }

    public string? ReadingTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public required string ThumbnailImageId { get; set; }

    public List<string>? ContentList { get; set; }
}