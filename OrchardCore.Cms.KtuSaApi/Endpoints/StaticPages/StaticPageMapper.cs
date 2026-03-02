using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public static class StaticPageMapper
{
    extension(ContentItem item)
    {
        public StaticPageResponse ToResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var part = item.As<StaticPagePart>();
            return new StaticPageResponse
            {
                Title = language.Resolve(part.TitleLt, part.TitleEn),
                Description = language.Resolve(part.DescriptionLt.Text, part.DescriptionEn.Text),
                ImgSrc = part.HeroImage.ToPublicUrl(mediaFileStore),
                Blocks = item.ToContentBlocks(language, mediaFileStore)
            };
        }
    }
}
