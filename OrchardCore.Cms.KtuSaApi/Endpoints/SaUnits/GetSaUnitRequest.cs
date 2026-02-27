using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class GetSaUnitRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;

    [Description("SA unit identifier. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK")]
    public SaUnit SaUnit { get; set; }
}
