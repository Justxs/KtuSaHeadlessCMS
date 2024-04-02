using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class EventPartViewModel
{
    [Required(ErrorMessage = "The Lithuanian title is required.")]
    public string TitleLt { get; set; } = null!;

    [Required(ErrorMessage = "The English title is required.")]
    public string TitleEn { get; set; } = null!;

    [Required(ErrorMessage = "Facebook event link is required")]
    public string FbEventLink { get; set; } = null!;

    public string? FientaTicketLink { get; set; }

    public string? Address { get; set; }

    [Required(ErrorMessage = "Event start date is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Event end date is required")]
    public DateTime EndDate { get; set; }
}