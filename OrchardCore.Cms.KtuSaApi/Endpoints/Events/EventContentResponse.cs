using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class EventContentResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("Event title in the requested language")]
    public required string Title { get; set; }

    [Description("URL of the Facebook event page")]
    public required string FacebookUrl { get; set; }

    [Description("URL of the Fienta ticket page; null if no tickets are available")]
    public string? FientaTicketUrl { get; set; }

    [Description("Physical address of the event; null if not set")]
    public string? Address { get; set; }

    [Description("Structured content blocks of the event body in the requested language")]
    public List<ContentBlockResponse>? Blocks { get; set; }

    [Description("Event start date and time (UTC)")]
    public DateTime StartDate { get; set; }

    [Description("Event end date and time (UTC)")]
    public DateTime EndDate { get; set; }

    [Description("File ID of the event cover image")]
    public required string CoverImageUrl { get; set; }

    [Description("Names of the SA units that organised this event")]
    public required List<string> Organisers { get; set; }
}