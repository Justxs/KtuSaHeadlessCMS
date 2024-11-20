using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Extensions;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ContactsController(IRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ContactDto>), 200)]
    public async Task<ActionResult> GetContactsBySaUnit(string language, [FromQuery] SaUnit saUnit)
    {
        var contacts = await repository.GetAllAsync(Contact);

        var isLithuanian = language.IsLtLanguage();

        var positions = await repository.GetAllAsync(Position);
        var saUnits = await repository.GetAllAsync(ContentTypeConstants.SaUnit);
        var saUnitId = saUnits.First(unit => unit.As<SaUnitPart>().UnitName == saUnit.ToString()).ContentItemId;


        var contactDtos = contacts
            .Where(item => item.As<MemberPart>().SaUnit.ContentItemIds.Contains(saUnitId))
            .Select(item =>
            {
                var memberPart = item.As<MemberPart>();

                var positionPart = positions
                    .FirstOrDefault(position => memberPart.Position.ContentItemIds
                        .Contains(position.ContentItemId))
                    .As<PositionPart>();

                var dto = new ContactDto
                {
                    Id = item.ContentItemId,
                    Email = memberPart.Email,

                    Name = memberPart.Name,
                    ImageSrc = memberPart.ImageUploadField.FileId,

                    Position = isLithuanian
                        ? positionPart.NameLt
                        : positionPart.NameEn,

                    Responsibilities = isLithuanian
                        ? positionPart.DescriptionLt
                        : positionPart.DescriptionEn,

                    Index = memberPart.Index
                };

                return dto;
            }).ToList();

        return Ok(contactDtos.OrderBy(contact=> contact.Index));
    }
}