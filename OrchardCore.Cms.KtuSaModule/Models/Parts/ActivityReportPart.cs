using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class ActivityReportPart : ContentPart
{
    public MediaField ReportFileLt { get; set; } = new();

    public MediaField ReportFileEn { get; set; } = new();

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public ContentPickerField SaUnit { get; set; } = null!;
}
