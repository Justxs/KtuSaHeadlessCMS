using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class ArticlePart : ContentPart
{
    public MediaField ThumbnailImage { get; set; } = new();

    public QuillField HtmlContentLt { get; set; } = null!;

    public QuillField HtmlContentEn { get; set; } = null!;
}
