using OrchardCore.Cms.KtuSaModule.Models.FIelds;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class SponsorPart : ContentPart
{
    public string Name { get; set; } = null!;

    public string WebsiteUrl { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}