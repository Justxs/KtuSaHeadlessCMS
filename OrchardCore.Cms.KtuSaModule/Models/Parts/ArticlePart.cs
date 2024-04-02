using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class ArticlePart : ContentPart
{
    public QuillField HtmlContentLt { get; set; } = null!;

    public QuillField HtmlContentEn { get; set;} = null!;
}