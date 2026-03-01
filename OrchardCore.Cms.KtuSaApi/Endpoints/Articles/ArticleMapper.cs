using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public static class ArticleMapper
{
    extension(ContentItem item)
    {
        public ArticlePreviewResponse ToPreviewResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var cardPart = item.As<CardPart>();
            var articlePart = item.As<ArticlePart>();
            return new ArticlePreviewResponse
            {
                Id = item.ContentItemId,
                Title = isLithuanian ? cardPart.TitleLt : cardPart.TitleEn,
                Preview = isLithuanian
                    ? articlePart.HtmlContentLt.HtmlBody.GetPreviewText()
                    : articlePart.HtmlContentEn.HtmlBody.GetPreviewText(),
                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = articlePart?.ThumbnailImage.ToPublicUrl(mediaFileStore) ?? string.Empty
            };
        }

        public ArticleContentResponse ToContentResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var cardPart = item.As<CardPart>();
            var articlePart = item.As<ArticlePart>();
            var response = new ArticleContentResponse
            {
                Id = item.ContentItemId,
                Title = (isLithuanian ? cardPart?.TitleLt : cardPart?.TitleEn)!,
                HtmlBody = (isLithuanian ? articlePart?.HtmlContentLt.HtmlBody : articlePart?.HtmlContentEn.HtmlBody)!,
                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = articlePart?.ThumbnailImage.ToPublicUrl(mediaFileStore) ?? string.Empty
            };
            response.ReadingTime = response.HtmlBody.CalculateReadingTime();
            response.ContentList = response.HtmlBody.GetContentList();
            return response;
        }
    }
}
