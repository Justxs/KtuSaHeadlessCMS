namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class DocumentResponse
{
    [Description("Document title in the requested language")]
    public required string Title { get; set; }

    [Description("File ID of the PDF document")]
    public required string PdfUrl { get; set; }
}
