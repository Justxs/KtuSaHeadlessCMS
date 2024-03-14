using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class PositionPartViewModel
{
    [Required(ErrorMessage = "The position name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The position description is required.")]
    public string Description { get; set; } = null!;
}