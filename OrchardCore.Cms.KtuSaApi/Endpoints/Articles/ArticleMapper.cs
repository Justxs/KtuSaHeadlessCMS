using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public static class ArticleMapper
{
    extension(ContentItem item)
    {
        public ArticlePreviewResponse ToPreviewResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var cardPart = item.As<CardPart>();
            var articlePart = item.As<ArticlePart>();
            return new ArticlePreviewResponse
            {
                Id = item.ContentItemId,
                Title = language.Resolve(cardPart.TitleLt, cardPart.TitleEn),
                Preview = item.GetCombinedHtml(language).GetPreviewText(),
                CreatedDate = item.CreatedUtc ?? DateTime.MinValue,
                ThumbnailImageUrl = articlePart?.ThumbnailImage.ToPublicUrl(mediaFileStore) ?? string.Empty
            };
        }

        public ArticleContentResponse ToContentResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var cardPart = item.As<CardPart>();
            var articlePart = item.As<ArticlePart>();
            var blocks = item.ToContentBlocks(language, mediaFileStore);
            var combinedHtml = item.GetCombinedHtml(language);
            return new ArticleContentResponse
            {
                Id = item.ContentItemId,
                Title = language.Resolve(cardPart?.TitleLt, cardPart?.TitleEn)!,
                Blocks = blocks,
                ReadingTime = combinedHtml.CalculateReadingTime(),
                ContentList = item.GetContentList(language),
                CreatedDate = item.CreatedUtc ?? DateTime.MinValue,
                ThumbnailImageUrl = articlePart?.ThumbnailImage.ToPublicUrl(mediaFileStore) ?? string.Empty
            };
        }
    }
}
