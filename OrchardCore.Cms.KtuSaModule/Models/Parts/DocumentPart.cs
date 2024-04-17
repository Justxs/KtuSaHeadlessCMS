using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class DocumentPart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public PdfUploadField DocumentLt { get; set; } = null!;

    public PdfUploadField DocumentEn { get; set; } = null!;

    public ContentPickerField CategoryField { get; set; } = null!;
}