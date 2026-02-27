using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public static class ActivityReportMapper
{
    public static ActivityReportResponse ToResponse(this ContentItem item, bool isLithuanian)
    {
        var part = item.As<ActivityReportPart>();
        return new ActivityReportResponse
        {
            Id = item.ContentItemId,
            PdfUrl = isLithuanian ? part.ReportLt.FileId : part.ReportEn.FileId,
            From = part.From,
            To = part.To
        };
    }
}