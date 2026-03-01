using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class StaticPagePart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public TextField DescriptionLt { get; set; } = null!;

    public TextField DescriptionEn { get; set; } = null!;

    public MediaField HeroImage { get; set; } = new();

    public QuillField BodyLt { get; set; } = null!;

    public QuillField BodyEn { get; set; } = null!;
}
