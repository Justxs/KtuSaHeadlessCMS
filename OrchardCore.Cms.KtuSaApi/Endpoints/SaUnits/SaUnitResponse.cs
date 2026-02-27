namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class SaUnitResponse
{
    [Description("File ID of the SA unit cover photo")]
    public required string CoverUrl { get; set; }

    [Description("SA unit description in the requested language")]
    public required string Description { get; set; }

    [Description("Contact email address")]
    public required string Email { get; set; }

    [Description("Contact phone number")]
    public required string PhoneNumber { get; set; }

    [Description("Physical address")]
    public required string Address { get; set; }

    [Description("URL of the SA unit's LinkedIn page")]
    public required string LinkedInUrl { get; set; }

    [Description("URL of the SA unit's Facebook page")]
    public required string FacebookUrl { get; set; }

    [Description("URL of the SA unit's Instagram page")]
    public required string InstagramUrl { get; set; }
}
