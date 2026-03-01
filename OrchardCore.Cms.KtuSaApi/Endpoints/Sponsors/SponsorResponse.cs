namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public class SponsorResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("Display name of the sponsor")]
    public required string Name { get; set; }

    [Description("URL of the sponsor's website")]
    public required string WebsiteUrl { get; set; }

    [Description("Public URL of the sponsor's logo image")]
    public required string LogoUrl { get; set; }
}