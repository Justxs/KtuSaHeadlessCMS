using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class ContactPartViewModel
{
    [Required(ErrorMessage = "The phone number is required.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "The email is required.")]
    public string Email { get; set; } = null!;
}