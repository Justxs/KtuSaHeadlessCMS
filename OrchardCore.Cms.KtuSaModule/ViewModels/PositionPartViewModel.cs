using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class PositionPartViewModel
{
    [Required(ErrorMessage = "The LT position name is required.")]
    public string NameLT { get; set; } = null!;

    [Required(ErrorMessage = "The EN position name is required.")]
    public string NameEN { get; set; } = null!;

    [Required(ErrorMessage = "The LT position description is required.")]
    public string DescriptionLT { get; set; } = null!;

    [Required(ErrorMessage = "The EN position description is required.")]
    public string DescriptionEN { get; set; } = null!;
}