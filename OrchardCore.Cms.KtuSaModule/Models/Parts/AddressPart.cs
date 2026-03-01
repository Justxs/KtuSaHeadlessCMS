using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class AddressPart : ContentPart
{
    public string Address { get; set; } = null!;

    public string SaUnit { get; set; } = null!;
}