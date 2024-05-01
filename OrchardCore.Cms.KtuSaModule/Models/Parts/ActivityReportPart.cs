using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class ActivityReportPart : ContentPart
{
    public PdfUploadField ReportLt { get; set; } = null!;

    public PdfUploadField ReportEn { get; set; } = null!;

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public ContentPickerField SaUnit { get; set; } = null!;
}