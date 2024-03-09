using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class ArticlePart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string PreviewLt { get; set; } = null!;

    public string PreviewEn { get; set;} = null!;

    public HtmlField HtmlContentLt { get; set; } = null!;

    public HtmlField HtmlContentEn { get; set;} = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}