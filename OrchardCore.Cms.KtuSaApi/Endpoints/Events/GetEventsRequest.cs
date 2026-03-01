using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";

    [QueryParam]
    [Description("When true, includes events whose start date is in the past")]
    public bool FetchPassed { get; set; }

    [QueryParam]
    [Description("Optional SA unit filter. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK")]
    public SaUnit? SaUnit { get; set; }
}