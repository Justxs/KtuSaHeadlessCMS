using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class AddressPart : ContentPart
{
    public string Address { get; set; } = null!;

    public SaUnitSelectField SaUnitSelectField { get; set; } = null!;
}