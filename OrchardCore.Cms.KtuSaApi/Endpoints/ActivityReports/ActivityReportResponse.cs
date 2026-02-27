namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public class ActivityReportResponse
{
    [Description("Content item ID")]
    public required string Id { get; set; }

    [Description("File ID of the activity report PDF in the requested language")]
    public required string PdfUrl { get; set; }

    [Description("Start date of the report period")]
    public required DateTime From { get; set; }

    [Description("End date of the report period")]
    public required DateTime To { get; set; }
}
