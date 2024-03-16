using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class PositionPart : ContentPart
{
    public string NameLt { get; set; } = null!;

    public string DescriptionLt { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string DescriptionEn { get; set; } = null!;
}