namespace OrchardCore.Cms.KtuSaModule.Dtos.Events;

public class EventPreviewDto
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required DateTime StartDate { get; set; }

    public required string CoverImageUrl { get; set; }
}