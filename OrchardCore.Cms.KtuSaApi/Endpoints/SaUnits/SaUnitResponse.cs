using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class SaUnitResponse
{
    [Description("File ID of the SA unit cover photo")]
    public required string CoverUrl { get; set; }

    [Description("Structured content blocks of the body in the requested language")]
    public List<ContentBlockResponse>? Blocks { get; set; }

    [Description("Contact email address")] public required string Email { get; set; }

    [Description("Contact phone number")] public required string PhoneNumber { get; set; }

    [Description("Physical address")] public required string Address { get; set; }

    [Description("URL of the SA unit's LinkedIn page")]
    public string? LinkedInUrl { get; set; }

    [Description("URL of the SA unit's Facebook page")]
    public string? FacebookUrl { get; set; }

    [Description("URL of the SA unit's Instagram page")]
    public string? InstagramUrl { get; set; }
}