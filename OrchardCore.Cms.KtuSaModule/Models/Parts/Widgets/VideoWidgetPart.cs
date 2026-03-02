using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;

public class VideoWidgetPart : ContentPart
{
    public TextField Url { get; set; } = null!;
}
