using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class StaticPagePartViewModel
{
    [Required(ErrorMessage = "The Lithuanian title is required.")]
    public string TitleLt { get; set; } = null!;

    [Required(ErrorMessage = "The English title is required.")]
    public string TitleEn { get; set; } = null!;
}