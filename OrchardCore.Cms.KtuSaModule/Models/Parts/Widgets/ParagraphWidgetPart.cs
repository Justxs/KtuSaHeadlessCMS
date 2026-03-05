using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;

public class ParagraphWidgetPart : ContentPart
{
    public HtmlField Body { get; set; } = new();
}