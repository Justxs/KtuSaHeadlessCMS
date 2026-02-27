using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public static class SaUnitMapper
{
    extension(ContentItem item)
    {
        public SaUnitResponse ToResponse(bool isLithuanian)
        {
            var saUnitPart = item.As<SaUnitPart>();
            var contactPart = item.As<ContactPart>();
            return new SaUnitResponse
            {
                CoverUrl = saUnitPart.SaPhoto.FileId,
                Description = isLithuanian ? saUnitPart.DescriptionLt : saUnitPart.DescriptionEn,
                LinkedInUrl = saUnitPart.LinkedInUrl,
                FacebookUrl = saUnitPart.FacebookUrl,
                InstagramUrl = saUnitPart.InstagramUrl,
                Address = saUnitPart.Address,
                Email = contactPart.Email,
                PhoneNumber = contactPart.PhoneNumber
            };
        }
    }
}