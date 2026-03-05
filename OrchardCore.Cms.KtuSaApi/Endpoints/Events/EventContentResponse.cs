using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class EventContentResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Event title in the requested language.")]
    public required string Title { get; set; }

    [Description("Facebook event URL.")]
    public required string FacebookUrl { get; set; }

    [Description("Fienta ticket URL; null when no ticket link is configured.")]
    public string? FientaTicketUrl { get; set; }

    [Description("Physical event address; null when not configured.")]
    public string? Address { get; set; }

    [Description("Structured content blocks of the event body in the requested language.")]
    public List<ContentBlockResponse>? Blocks { get; set; }

    [Description("Event start date and time as stored in the CMS.")]
    public DateTime StartDate { get; set; }

    [Description("Event end date and time as stored in the CMS.")]
    public DateTime EndDate { get; set; }

    [Description("Public URL of the event cover image.")]
    public required string CoverImageUrl { get; set; }

    [Description("SA unit codes that organized the event.")]
    public required List<string> Organisers { get; set; }
}
