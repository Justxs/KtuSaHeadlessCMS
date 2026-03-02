using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class GetContactsRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("saUnit")]
    [Description(
        "SA unit identifier. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK")]
    public SaUnit SaUnit { get; set; }
}