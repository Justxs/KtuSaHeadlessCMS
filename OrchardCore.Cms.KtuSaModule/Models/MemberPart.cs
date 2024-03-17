using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class MemberPart : ContentPart
{
    public string Name { get; set; } = null!;

    public SaUnitSelectField SaUnitSelectField { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;
}