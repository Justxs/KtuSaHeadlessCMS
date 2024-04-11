using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class SaUnitPart : ContentPart
{
    public ImageUploadField SaPhoto { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public string DescriptionLt { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string LinkedInUrl { get; set; } = null!;

    public string FacebookUrl { get; set; } = null!;

    public string InstagramUrl { get; set; } = null!;

}