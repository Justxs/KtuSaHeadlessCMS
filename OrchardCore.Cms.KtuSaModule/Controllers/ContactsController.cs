using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ContactsController(
    ISession session, 
    IStringActionService stringActionService,
    IRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ContactDto>), 200)]
    public async Task<ActionResult> GetContactsBySaUnit(string language, [FromQuery] SaUnit saUnit)
    {
        var contacts = await session
            .Query<ContentItem, MemberPartIndex>(index => index.SaUnit == saUnit)
            .ListAsync();

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var positions = await repository.GetAllAsync(nameof(ContentTypeNames.Position));

        var contactDtos = contacts.Select(item =>
        {
            var contactPart = item.As<ContactPart>();
            var memberPart = item.As<MemberPart>();

            var positionPart = positions
                .FirstOrDefault(position => memberPart.Position.ContentItemIds
                    .Contains(position.ContentItemId))
                .As<PositionPart>();

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