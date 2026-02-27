namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public class SponsorResponse
{
    [Description("Content item ID")] public string Id { get; set; } = null!;

    [Description("Display name of the sponsor")]
    public string Name { get; set; } = null!;

    [Description("URL of the sponsor's website")]
    public string WebsiteUrl { get; set; } = null!;

    [Description("File ID of the sponsor's logo image")]
    public string LogoId { get; set; } = null!;
}