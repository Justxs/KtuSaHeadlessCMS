namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class EventDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string FacebookUrl { get; set; } = null!;

    public string? FientaTicketUrl { get; set; }
    
    public string? Address { get; set; }

    public string? HtmlBody { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string CoverImageUrl { get; set; } = null!;

    public List<string> Organisers { get; set; } = null!;
}