using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class EventPart : ContentPart
{
    public string FbEventLink { get; set; } = null!;

    public DateTime Date { get; set; }

    public QuillField BodyFieldLt { get; set; } = null!;

    public QuillField BodyFieldEn { get; set; } = null!;

    public SaUnitSelectField SaUnit { get; set; } = null!;
}