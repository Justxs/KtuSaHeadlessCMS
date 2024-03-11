using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class DukPart : ContentPart
{
    public string QuestionLt { get; set; } = null!;

    public string QuestionEn { get; set; } = null!;

    public string AnswerLt { get; set; } = null!;

    public string AnswerEn { get; set; } = null!;
}