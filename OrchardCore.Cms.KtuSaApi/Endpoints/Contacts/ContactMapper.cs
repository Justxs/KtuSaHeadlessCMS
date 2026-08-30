using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public static class ContactMapper
{
    extension(ContentItem item)
    {
        public ContactResponse ToResponse(Language language, IEnumerable<ContentItem> positions,
            IMediaFileStore mediaFileStore)
        {
        var memberPart = item.GetOrCreate<MemberPart>();
            var positionItem = positions
                .FirstOrDefault(p => memberPart.Position.ContentItemIds.Contains(p.ContentItemId));
        var positionPart = positionItem?.GetOrCreate<PositionPart>();

            return new ContactResponse
            {
                Id = item.ContentItemId,
                Name = memberPart.Name,
                Email = memberPart.Email,
                ImageSrc = memberPart.MemberPhoto.ToPublicUrl(mediaFileStore),
                Position = positionPart is not null
                    ? language.Resolve(positionPart.NameLt, positionPart.NameEn)
                    : string.Empty,
                Responsibilities = positionPart is not null
                    ? language.Resolve(positionPart.DescriptionLt, positionPart.DescriptionEn)
                    : string.Empty,
                Index = memberPart.Index
            };
        }
    }
}
