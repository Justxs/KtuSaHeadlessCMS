using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public static class StaticPageMapper
{
    extension(ContentItem item)
    {
        public StaticPageResponse ToResponse(bool isLithuanian, IMediaFileStore mediaFileStore)
        {
            var part = item.As<StaticPagePart>();
            return new StaticPageResponse
            {
                Title = isLithuanian ? part.TitleLt : part.TitleEn,
                Description = isLithuanian ? part.DescriptionLt?.Text : part.DescriptionEn?.Text,
                ImgSrc = part.HeroImage.ToPublicUrl(mediaFileStore),
                Body = isLithuanian ? part.BodyLt?.HtmlBody : part.BodyEn?.HtmlBody
            };
        }
    }
}
