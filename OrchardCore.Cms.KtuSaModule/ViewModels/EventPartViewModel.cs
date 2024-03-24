using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels;

public class EventPartViewModel
{
    [Required(ErrorMessage = "Facebook event link is required")]
    public string FbEventLink { get; set; } = null!;
}