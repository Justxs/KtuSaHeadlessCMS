using OrchardCore.Cms.KtuSaModule.Models.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class CardPart : ContactPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string PreviewLt { get; set; } = null!;

    public string PreviewEn { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}