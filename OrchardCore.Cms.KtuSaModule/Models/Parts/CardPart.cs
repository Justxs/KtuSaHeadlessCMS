using OrchardCore.Cms.KtuSaModule.Models.FIelds;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class CardPart : ContactPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string PreviewLt { get; set; } = null!;

    public string PreviewEn { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}