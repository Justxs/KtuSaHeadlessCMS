namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class ActivityReportDto
{
    public required string Id { get; set; }

    public required string PdfUrl { get; set; }

    public required DateTime From { get; set; }

    public required DateTime To { get; set; }
}