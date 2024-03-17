using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class HeroSectionPart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public TextField DescriptionLt { get; set; } = null!;

    public TextField DescriptionEn { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}