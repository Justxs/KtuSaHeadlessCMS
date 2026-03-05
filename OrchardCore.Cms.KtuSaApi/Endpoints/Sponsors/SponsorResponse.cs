namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public class SponsorResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Display name of the sponsor.")]
    public required string Name { get; set; }

    [Description("Sponsor website URL.")]
    public required string WebsiteUrl { get; set; }

    [Description("Public URL of the sponsor logo image.")]
    public required string LogoUrl { get; set; }
}
