namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class EventPreviewResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Event title in the requested language.")]
    public required string Title { get; set; }

    [Description("Event start date and time as stored in the CMS.")]
    public required DateTime StartDate { get; set; }

    [Description("Public URL of the event cover image.")]
    public required string CoverImageUrl { get; set; }
}