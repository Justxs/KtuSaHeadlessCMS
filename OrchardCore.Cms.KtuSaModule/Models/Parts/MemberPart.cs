using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class MemberPart : ContentPart
{
    public string Name { get; set; } = null!;

    public SaUnitSelectField SaUnitSelectField { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;

    public ContentPickerField Position { get; set; } = null!;

}