namespace OrchardCore.Cms.KtuSaModule.Dtos.Articles;

public class ArticlePreviewDto
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string Preview { get; set; }

    public DateTime CreatedDate { get; set; }

    public required string ThumbnailImageId { get; set; }
}