using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsRequest
{
    [QueryParam]
    [Description("Response language. Allowed values: 'en' (default) and 'lt'.")]
    public Language Language { get; set; } = Language.EN;

    [QueryParam]
    [Description(
        "Optional student association unit filter. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")]
    public SaUnit? SaUnit { get; set; }
}
