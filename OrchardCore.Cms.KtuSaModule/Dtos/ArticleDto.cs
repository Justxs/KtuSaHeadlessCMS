namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class ArticleDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Preview { get; set; } = null!;

    public string? HtmlBody { get; set; }

    public string? ReadingTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ThumbnailImageId { get; set; } = null!;
}