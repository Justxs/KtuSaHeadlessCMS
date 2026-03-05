using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class ContactPart : ContentPart
{
    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public MediaField Photo { get; set; } = new();
}