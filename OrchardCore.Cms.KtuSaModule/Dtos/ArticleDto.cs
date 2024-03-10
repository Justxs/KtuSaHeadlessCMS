namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class ArticleDto
{
    public string Title { get; set; } = null!;

    public string Preview { get; set; } = null!;

    public string HtmlBody { get; set; } = null!;

    public string ReadingTime { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string ThumbnailImageId { get; set; } = null!;
}