using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class SponsorPart : ContentPart
{
    public string Name { get; set; } = null!;

    public string WebsiteUrl { get; set; } = null!;

    public MediaField Logo { get; set; } = new();
}
