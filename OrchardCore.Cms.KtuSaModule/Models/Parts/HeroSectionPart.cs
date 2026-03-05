using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class HeroSectionPart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public TextField DescriptionLt { get; set; } = null!;

    public TextField DescriptionEn { get; set; } = null!;

    public MediaField ImageUploadField { get; set; } = new();
}