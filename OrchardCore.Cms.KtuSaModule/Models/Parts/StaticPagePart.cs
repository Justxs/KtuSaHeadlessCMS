using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class StaticPagePart : ContentPart 
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public QuillField BodyLt { get; set; } = null!;

    public QuillField BodyEn { get; set; } = null!;
}