using OrchardCore.Cms.KtuSaModule.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class MemberPartViewModel
{
    [Required(ErrorMessage = "The member name is required.")]
    public string Name { get; set; } = null!;
}