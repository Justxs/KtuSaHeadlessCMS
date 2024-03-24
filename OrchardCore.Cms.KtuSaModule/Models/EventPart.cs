using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class EventPart : ContentPart
{
    public string FbEventLink { get; set; } = null!;

    public QuillField BodyFieldLt { get; set; } = null!;

    public QuillField BodyFieldEn { get; set; } = null!;

    public SaUnitSelectField SaUnit { get; set; } = null!;
}