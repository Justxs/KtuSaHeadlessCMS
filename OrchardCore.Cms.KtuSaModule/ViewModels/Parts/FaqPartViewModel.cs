using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class FaqPartViewModel
{
    [Required(ErrorMessage = "The Lithuanian question is required.")]
    public string QuestionLt { get; set; } = null!;

    [Required(ErrorMessage = "The English question is required.")]
    public string QuestionEn { get; set; } = null!;
}
