namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class EventDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Preview { get; set; } = null!;

    public string? HtmlBody { get; set; }

    public DateTime Date { get; set; }

    public string ThumbnailImageId { get; set; } = null!;
}