using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class SponsorPartViewModel
{
    [Required(ErrorMessage = "The sponsor name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "The website url is required.")]
    public string WebsiteUrl { get; set; } = null!;
}