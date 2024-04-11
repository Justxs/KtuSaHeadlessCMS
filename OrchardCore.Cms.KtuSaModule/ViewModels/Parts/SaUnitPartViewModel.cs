using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class SaUnitPartViewModel
{
    [Required(ErrorMessage = "The unit name is required.")]
    public string UnitName { get; set; } = null!;

    [Required(ErrorMessage = "The LT description is required.")]
    public string DescriptionLt { get; set; } = null!;

    [Required(ErrorMessage = "The EN description is required.")]
    public string DescriptionEn { get; set; } = null!;

    [Required(ErrorMessage = "The address is required.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "The LinkedIn Url is required.")]
    public string LinkedInUrl { get; set; } = null!;

    [Required(ErrorMessage = "The Facebook Url is required.")]
    public string FacebookUrl { get; set; } = null!;

    [Required(ErrorMessage = "The Instagram Url is required.")]
    public string InstagramUrl { get; set; } = null!;
}