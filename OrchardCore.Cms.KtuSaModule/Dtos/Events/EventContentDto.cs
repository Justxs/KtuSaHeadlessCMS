namespace OrchardCore.Cms.KtuSaModule.Dtos.Events;

public class EventContentDto
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string FacebookUrl { get; set; }

    public string? FientaTicketUrl { get; set; }

    public string? Address { get; set; }

    public string? HtmlBody { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public required string CoverImageUrl { get; set; }

    public required List<string> Organisers { get; set; }
}