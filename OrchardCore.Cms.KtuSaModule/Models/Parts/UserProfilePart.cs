using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class UserProfilePart : ContentPart
{
    public ContentPickerField SaUnit { get; set; } = null!;
}