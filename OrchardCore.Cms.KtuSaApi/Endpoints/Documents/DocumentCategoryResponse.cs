namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class DocumentCategoryResponse
{
    [Description("Category name in the requested language")]
    public required string Category { get; set; }

    [Description("Documents belonging to this category")]
    public required List<DocumentResponse> Documents { get; set; }
}
