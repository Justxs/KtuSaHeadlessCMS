using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class SaUnitResponse
{
    [Description("Public URL of the SA unit cover photo.")]
    public required string CoverUrl { get; set; }

    [Description("Structured content blocks in the requested language.")]
    public List<ContentBlockResponse>? Blocks { get; set; }

    [Description("Contact email address.")]
    public required string Email { get; set; }

    [Description("Contact phone number.")]
    public required string PhoneNumber { get; set; }

    [Description("Physical address.")]
    public required string Address { get; set; }

    [Description("LinkedIn profile/page URL.")]
    public string? LinkedInUrl { get; set; }

    [Description("Facebook profile/page URL.")]
    public string? FacebookUrl { get; set; }

    [Description("Instagram profile/page URL.")]
    public string? InstagramUrl { get; set; }
}
