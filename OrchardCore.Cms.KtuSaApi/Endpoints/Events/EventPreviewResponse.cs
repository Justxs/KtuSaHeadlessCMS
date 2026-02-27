namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class EventPreviewResponse
{
    [Description("Content item ID")]
    public required string Id { get; set; }

    [Description("Event title in the requested language")]
    public required string Title { get; set; }

    [Description("Event start date and time (UTC)")]
    public required DateTime StartDate { get; set; }

    [Description("File ID of the event cover image")]
    public required string CoverImageUrl { get; set; }
}
