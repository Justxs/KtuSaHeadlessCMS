using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;

public class ImageWidgetPart : ContentPart
{
    public MediaField Image { get; set; } = new();
}
