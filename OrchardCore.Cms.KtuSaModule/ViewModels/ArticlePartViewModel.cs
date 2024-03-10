using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class ArticlePartViewModel
{
    [Required(ErrorMessage = "The Lithuanian title is required.")]
    public string TitleLt { get; set; } = null!;

    [Required(ErrorMessage = "The English title is required.")]
    public string TitleEn { get; set; } = null!;

    [Required(ErrorMessage = "The Lithuanian preview is required.")]
    public string PreviewLt { get; set; } = null!;

    [Required(ErrorMessage = "The English preview is required.")]
    public string PreviewEn { get; set; } = null!;
}