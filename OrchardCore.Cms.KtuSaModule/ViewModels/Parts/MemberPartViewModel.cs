using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class MemberPartViewModel
{
    [Required(ErrorMessage = "The member name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The email is required.")]
    public string Email { get; set; } = null!;

    public int Index { get; set; } = 1;

    public string? SaUnit { get; set; }
}