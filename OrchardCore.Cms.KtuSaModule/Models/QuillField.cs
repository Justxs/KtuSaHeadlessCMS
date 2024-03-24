using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Models;

public class QuillField : ContentField
{
    public string HtmlBody { get; set; } = null!;
}