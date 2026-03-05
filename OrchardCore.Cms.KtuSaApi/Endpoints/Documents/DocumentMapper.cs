using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public static class DocumentMapper
{
    extension(ContentItem item)
    {
        public DocumentCategoryResponse ToCategoryResponse(Language language,
            IEnumerable<ContentItem> documents,
            IMediaFileStore mediaFileStore)
        {
            var category = item.As<CategoryPart>();
            return new DocumentCategoryResponse
            {
                Category = language.Resolve(category.TitleLt, category.TitleEn),
                Documents =
                [
                    .. documents
                        .Where(d => d.As<ContainedPart>()?.ListContentItemId == item.ContentItemId)
                        .Select(d => d.ToResponse(language, mediaFileStore))
                ]
            };
        }

        public DocumentResponse ToResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var document = item.As<DocumentPart>();
            return new DocumentResponse
            {
                Title = language.Resolve(document.TitleLt, document.TitleEn),
                PdfUrl = language.Resolve(document.FileLt, document.FileEn).ToPublicUrl(mediaFileStore)
            };
        }
    }
}