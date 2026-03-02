using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;

public class PdfDocumentWidgetPart : ContentPart
{
    public MediaField Document { get; set; } = new();
}
