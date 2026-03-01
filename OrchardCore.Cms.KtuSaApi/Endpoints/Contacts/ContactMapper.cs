using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public static class ContactMapper
{
    extension(ContentItem item)
    {
        public ContactResponse ToResponse(bool isLithuanian, IEnumerable<ContentItem> positions, IMediaFileStore mediaFileStore)
        {
            var memberPart = item.As<MemberPart>();
            var positionPart = positions
                .FirstOrDefault(p => memberPart.Position.ContentItemIds.Contains(p.ContentItemId))
                .As<PositionPart>();

            return new ContactResponse
            {
                Id = item.ContentItemId,
                Name = memberPart.Name,
                Email = memberPart.Email,
                ImageSrc = memberPart.MemberPhoto.ToPublicUrl(mediaFileStore),
                Position = isLithuanian ? positionPart.NameLt : positionPart.NameEn,
                Responsibilities = isLithuanian ? positionPart.DescriptionLt : positionPart.DescriptionEn,
                Index = memberPart.Index
            };
        }
    }
}
