using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class MemberPart : ContentPart
{
    public string Name { get; set; } = null!;

    public SaUnit SaUnit { get; set; }

    public ImageUploadField ImageUploadField { get; set; } = null!;
}