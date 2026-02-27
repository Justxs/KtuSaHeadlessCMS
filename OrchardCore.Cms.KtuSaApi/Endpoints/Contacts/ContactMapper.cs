using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public static class ContactMapper
{
    public static ContactResponse ToResponse(this ContentItem item, bool isLithuanian,
        IEnumerable<ContentItem> positions)
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
            ImageSrc = memberPart.ImageUploadField.FileId,
            Position = isLithuanian ? positionPart.NameLt : positionPart.NameEn,
            Responsibilities = isLithuanian ? positionPart.DescriptionLt : positionPart.DescriptionEn,
            Index = memberPart.Index
        };
    }
}