using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class GetMainContactsRequest
{
    [BindFrom("saUnit")]
    [Description(
        "Student association unit code. Allowed values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")]
    public SaUnit SaUnit { get; set; }
}