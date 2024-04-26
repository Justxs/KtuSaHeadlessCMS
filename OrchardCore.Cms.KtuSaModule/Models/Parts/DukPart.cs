using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class DukPart : ContentPart
{
    public string QuestionLt { get; set; } = null!;

    public string QuestionEn { get; set; } = null!;

    public string AnswerLt { get; set; } = null!;

    public string AnswerEn { get; set; } = null!;
}