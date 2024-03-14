using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class PositionPart : ContentPart
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}