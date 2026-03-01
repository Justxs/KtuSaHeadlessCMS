using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class MemberPart : ContentPart
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Index { get; set; } = 1;

    public ContentPickerField SaUnit { get; set; } = null!;

    public MediaField MemberPhoto { get; set; } = new();

    public ContentPickerField Position { get; set; } = null!;
}
