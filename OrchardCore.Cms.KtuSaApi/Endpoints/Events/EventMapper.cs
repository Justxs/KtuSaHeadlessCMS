using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public static class EventMapper
{
    extension(ContentItem item)
    {
        public EventPreviewResponse ToPreviewResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var part = item.As<EventPart>();
            return new EventPreviewResponse
            {
                Id = item.ContentItemId,
                Title = isLithuanian ? part.TitleLt : part.TitleEn,
                StartDate = part.StartDate,
                CoverImageUrl = part.CoverImage.ToPublicUrl(mediaFileStore)
            };
        }

        public EventContentResponse ToContentResponse(bool isLithuanian,
            List<string> organisers,
            IMediaFileStore mediaFileStore)
        {
            var part = item.As<EventPart>();
            return new EventContentResponse
            {
                Id = item.ContentItemId,
                Title = isLithuanian ? part.TitleLt : part.TitleEn,
                HtmlBody = isLithuanian ? part.BodyFieldLt.HtmlBody : part.BodyFieldEn.HtmlBody,
                FientaTicketUrl = ParseFientaUrl(isLithuanian ? part.FientaTicketLinkLt : part.FientaTicketLinkEn,
                    isLithuanian),
                StartDate = part.StartDate,
                EndDate = part.EndDate,
                Address = part.Address,
                FacebookUrl = part.FbEventLink,
                CoverImageUrl = part.CoverImage.ToPublicUrl(mediaFileStore),
                Organisers = organisers
            };
        }
    }

    private static string? ParseFientaUrl(string? raw, bool isLithuanian)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var parts = raw.Split("|||", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => null,
            1 => parts[0],
            _ => isLithuanian ? parts[0] : parts[^1]
        };
    }
}
