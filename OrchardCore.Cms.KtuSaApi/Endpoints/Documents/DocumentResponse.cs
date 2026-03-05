namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class DocumentResponse
{
    [Description("Document title in the requested language.")]
    public required string Title { get; set; }

    [Description("Public URL of the PDF document in the requested language.")]
    public required string PdfUrl { get; set; }
}