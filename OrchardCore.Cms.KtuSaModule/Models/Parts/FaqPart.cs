using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class FaqPart : ContentPart
{
    public string QuestionLt { get; set; } = null!;

    public string QuestionEn { get; set; } = null!;
}