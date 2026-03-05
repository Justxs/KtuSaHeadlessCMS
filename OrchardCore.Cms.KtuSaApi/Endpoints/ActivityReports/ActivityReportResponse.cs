namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public class ActivityReportResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Public URL of the activity report PDF in the requested language.")]
    public required string PdfUrl { get; set; }

    [Description("Report period start date.")]
    public required DateTime From { get; set; }

    [Description("Report period end date.")]
    public required DateTime To { get; set; }
}