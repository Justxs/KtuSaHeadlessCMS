using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class ArticlePart : ContentPart
{
    public MediaField ThumbnailImage { get; set; } = new();
}