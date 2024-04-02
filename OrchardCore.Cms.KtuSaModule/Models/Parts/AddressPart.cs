using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class AddressPart : ContentPart
{
    public string Address { get; set; } = null!;

    public SaUnitSelectField SaUnitSelectField { get; set; } = null!;
}