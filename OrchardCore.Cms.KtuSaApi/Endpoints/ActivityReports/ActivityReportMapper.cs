using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public static class ActivityReportMapper
{
    extension(ContentItem item)
    {
        public ActivityReportResponse ToResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var part = item.As<ActivityReportPart>();
            return new ActivityReportResponse
            {
                Id = item.ContentItemId,
                PdfUrl = (isLithuanian ? part.ReportFileLt : part.ReportFileEn).ToPublicUrl(mediaFileStore),
                From = part.From,
                To = part.To
            };
        }
    }
}
