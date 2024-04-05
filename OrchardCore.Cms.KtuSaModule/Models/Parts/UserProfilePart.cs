using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class UserProfilePart : ContentPart
{
    public SaUnitSelectField SaUnit { get; set; } = null!;
}