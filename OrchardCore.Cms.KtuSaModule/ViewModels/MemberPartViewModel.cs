using OrchardCore.Cms.KtuSaModule.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class MemberPartViewModel
{
    [Required(ErrorMessage = "The member name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The sa unit is required.")]
    public SaUnit SaUnit { get; set; }
}