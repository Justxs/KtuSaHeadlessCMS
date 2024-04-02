using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models.Fields;

public class QuillField : ContentField
{
    public string HtmlBody { get; set; } = null!;
}