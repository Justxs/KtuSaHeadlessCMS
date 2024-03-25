using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Services;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ContactsController(IContentManager contentManager, ISession session, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string ContactContentType = ContentTypeNames.Contact.ToString();

    [HttpGet]
    public async Task<ActionResult> GetContactsBySaUnit(string language, [FromQuery] SaUnit saUnit)
    {
        var contacts = await session
            .Query<ContentItem, MemberPartIndex>(index => index.SaUnit == saUnit)
            .ListAsync();

        foreach (var contact in contacts)
        {
            await contentManager.LoadAsync(contact);
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var contactDtos = contacts.Select(item =>
        {
            var contactPart = item.As<ContactPart>();
            var memberPart = item.As<MemberPart>();
            var positionPart = item.As<PositionPart>();

            var dto = new ContactDto
            {
                Id = item.ContentItemId,
                PhoneNumber = contactPart?.PhoneNumber!,
                Email = contactPart?.Email!,

                Name = memberPart?.Name!,
                ImageSrc = memberPart?.ImageUploadField.FileId!,

                Position = (isLithuanian
                    ? positionPart?.NameLt
                    : positionPart?.NameEn)!,

                Responsibilities = (isLithuanian
                    ? positionPart?.DescriptionLt
                    : positionPart?.DescriptionEn)!,

            };

            return dto;
        }).ToList();

        return Ok(contactDtos);
    }
}