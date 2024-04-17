namespace OrchardCore.Cms.KtuSaModule.Dtos.Documents;

public class DocumentsCategoriesDto
{
    public required string Category { get; set; }

    public required List<DocumentsDto> Documents { get; set; }
}