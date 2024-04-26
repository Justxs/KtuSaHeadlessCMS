using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class MemberPartViewModel
{
    [Required(ErrorMessage = "The member name is required.")]
    public string Name { get; set; } = null!;

    public string? SaUnit { get; set; }
}