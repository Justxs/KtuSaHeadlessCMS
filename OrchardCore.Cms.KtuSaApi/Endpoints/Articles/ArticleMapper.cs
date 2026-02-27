using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public static class ArticleMapper
{
    extension(ContentItem item)
    {
        public ArticlePreviewResponse ToPreviewResponse(bool isLithuanian)
        {
            var part = item.As<CardPart>();
            var htmlPart = item.As<ArticlePart>();
            return new ArticlePreviewResponse
            {
                Id = item.ContentItemId,
                Title = isLithuanian ? part.TitleLt : part.TitleEn,
                Preview = isLithuanian
                    ? htmlPart.HtmlContentLt.HtmlBody.GetPreviewText()
                    : htmlPart.HtmlContentEn.HtmlBody.GetPreviewText(),
                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = part.ImageUploadField.FileId
            };
        }

        public ArticleContentResponse ToContentResponse(bool isLithuanian)
        {
            var part = item.As<CardPart>();
            var htmlPart = item.As<ArticlePart>();
            var response = new ArticleContentResponse
            {
                Id = item.ContentItemId,
                Title = (isLithuanian ? part?.TitleLt : part?.TitleEn)!,
                HtmlBody = (isLithuanian ? htmlPart?.HtmlContentLt.HtmlBody : htmlPart?.HtmlContentEn.HtmlBody)!,
                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = part!.ImageUploadField.FileId
            };
            response.ReadingTime = response.HtmlBody.CalculateReadingTime();
            response.ContentList = response.HtmlBody.GetContentList();
            return response;
        }
    }
}