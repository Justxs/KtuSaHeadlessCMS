using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public static class EventMapper
{
    extension(ContentItem item)
    {
        public EventPreviewResponse ToPreviewResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var part = item.As<EventPart>();
            return new EventPreviewResponse
            {
                Id = item.ContentItemId,
                Title = language.Resolve(part.TitleLt, part.TitleEn),
                StartDate = part.StartDate,
                CoverImageUrl = part.CoverImage.ToPublicUrl(mediaFileStore)
            };
        }

        public EventContentResponse ToContentResponse(Language language,
            List<string> organisers,
            IMediaFileStore mediaFileStore)
        {
            var part = item.As<EventPart>();
            return new EventContentResponse
            {
                Id = item.ContentItemId,
                Title = language.Resolve(part.TitleLt, part.TitleEn),
                Blocks = item.ToContentBlocks(language, mediaFileStore),
                FientaTicketUrl = ParseFientaUrl(language.Resolve(part.FientaTicketLinkLt, part.FientaTicketLinkEn)),
                StartDate = part.StartDate,
                EndDate = part.EndDate,
                Address = part.Address,
                FacebookUrl = part.FbEventLink,
                CoverImageUrl = part.CoverImage.ToPublicUrl(mediaFileStore),
                Organisers = organisers
            };
        }
    }

    private static string? ParseFientaUrl(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var parts = raw.Split("|||", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => null,
            _ => parts[0]
        };
    }
}