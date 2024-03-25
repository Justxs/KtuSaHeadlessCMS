using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class DukPartViewModel
{
    [Required(ErrorMessage = "The Lithuanian question is required.")]
    public string QuestionLt { get; set; } = null!;

    [Required(ErrorMessage = "The English question is required.")]
    public string QuestionEn { get; set; } = null!;

    [Required(ErrorMessage = "The Lithuanian answer is required.")]
    public string AnswerLt { get; set; } = null!;

    [Required(ErrorMessage = "The English answer is required.")]
    public string AnswerEn { get; set; } = null!;
}