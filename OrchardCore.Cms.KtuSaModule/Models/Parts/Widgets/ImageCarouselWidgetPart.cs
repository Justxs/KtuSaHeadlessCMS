using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;

public class ImageCarouselWidgetPart : ContentPart
{
    public MediaField Images { get; set; } = new();
}
