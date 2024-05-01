using System.ComponentModel.DataAnnotations;

namespace OrchardCore.Cms.KtuSaModule.ViewModels.Parts;

public class ActivityReportPartViewModel
{
    [Required(ErrorMessage = "Activity report from date is required")]
    public DateTime From { get; set; }

    [Required(ErrorMessage = "Activity report to date is required")]
    public DateTime To { get; set; }
}