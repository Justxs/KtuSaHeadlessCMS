using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public static class SponsorMapper
{
    extension(ContentItem item)
    {
        public SponsorResponse ToResponse(IMediaFileStore mediaFileStore)
        {
            var part = item.As<SponsorPart>();
            return new SponsorResponse
            {
                Id = item.ContentItemId,
                Name = part.Name,
                WebsiteUrl = part.WebsiteUrl,
                LogoUrl = part.Logo.ToPublicUrl(mediaFileStore)
            };
        }
    }
}
