using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public static class SaUnitMapper
{
    extension(ContentItem item)
    {
        public SaUnitResponse ToResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var saUnitPart = item.As<SaUnitPart>();
            return new SaUnitResponse
            {
                CoverUrl = saUnitPart.UnitPhoto.ToPublicUrl(mediaFileStore),
                Blocks = item.ToContentBlocks(language, mediaFileStore),
                LinkedInUrl = saUnitPart.LinkedInUrl,
                FacebookUrl = saUnitPart.FacebookUrl,
                InstagramUrl = saUnitPart.InstagramUrl,
                Address = saUnitPart.Address,
                Email = saUnitPart.Email,
                PhoneNumber = saUnitPart.PhoneNumber
            };
        }
    }
}
