using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class PositionPartViewModel
{
    [Required(ErrorMessage = "The LT position name is required.")]
    public string NameLt { get; set; } = null!;

    [Required(ErrorMessage = "The EN position name is required.")]
    public string NameEn { get; set; } = null!;

    [Required(ErrorMessage = "The LT position description is required.")]
    public string DescriptionLt { get; set; } = null!;

    [Required(ErrorMessage = "The EN position description is required.")]
    public string DescriptionEn { get; set; } = null!;
}