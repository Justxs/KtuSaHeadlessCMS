using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public static class SponsorMapper
{
    extension(ContentItem item)
    {
        public SponsorResponse ToResponse()
        {
            var part = item.As<SponsorPart>();
            return new SponsorResponse
            {
                Id = item.ContentItemId,
                Name = part.Name,
                WebsiteUrl = part.WebsiteUrl,
                LogoId = part.ImageUploadField.FileId
            };
        }
    }
}