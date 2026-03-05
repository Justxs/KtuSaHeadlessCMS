using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class SaUnitPartViewModel
{
    [Required(ErrorMessage = "The unit name is required.")]
    public string UnitName { get; set; } = null!;

    [Required(ErrorMessage = "The address is required.")]
    public string Address { get; set; } = null!;

    public string? LinkedInUrl { get; set; }

    public string? FacebookUrl { get; set; }

    public string? InstagramUrl { get; set; }

    [Required(ErrorMessage = "The phone number is required.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "The email is required.")]
    public string Email { get; set; } = null!;
}