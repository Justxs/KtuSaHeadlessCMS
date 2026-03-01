using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public static class DocumentMapper
{
    extension(ContentItem item)
    {
        public DocumentCategoryResponse ToCategoryResponse(bool isLithuanian,
            IEnumerable<ContentItem> documents,
            IMediaFileStore mediaFileStore)
        {
            var category = item.As<CategoryPart>();
            return new DocumentCategoryResponse
            {
                Category = isLithuanian ? category.TitleLt : category.TitleEn,
                Documents = [.. documents
                    .Where(d => d.As<DocumentPart>().CategoryField.ContentItemIds.Contains(item.ContentItemId))
                    .Select(d => d.ToResponse(isLithuanian, mediaFileStore))]
            };
        }

        public DocumentResponse ToResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var document = item.As<DocumentPart>();
            return new DocumentResponse
            {
                Title = isLithuanian ? document.TitleLt : document.TitleEn,
                PdfUrl = (isLithuanian ? document.FileLt : document.FileEn).ToPublicUrl(mediaFileStore)
            };
        }
    }
}
