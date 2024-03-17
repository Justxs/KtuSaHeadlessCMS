namespace OrchardCore.Cms.KtuSaModule.Models;

public class AddressPart : ContactPart
{
    public string Address { get; set; } = null!;

    public SaUnitSelectField SaUnitSelectField { get; set; } = null!;
}