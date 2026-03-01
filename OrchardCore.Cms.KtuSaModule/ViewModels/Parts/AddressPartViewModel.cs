using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class AddressPartViewModel
{
    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;
}