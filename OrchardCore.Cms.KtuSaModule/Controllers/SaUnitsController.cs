using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos.SaUnits;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class SaUnitsController(IRepository repository, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string SaUnitContentType = ContentTypeNames.SaUnit.ToString();


    [HttpGet("{saUnit}")]
    [ProducesResponseType(typeof(SaUnitDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<ActionResult> GetEventsByFsa(string language, SaUnit saUnit)
    {
        var saUnits = await repository.GetAllAsync(SaUnitContentType);
        var filterSaUnit = saUnits.FirstOrDefault(item => item.As<SaUnitPart>().UnitName == saUnit.ToString());

        if (filterSaUnit == null)
        {
            return NotFound("Sa unit not found");
        }

        var saUnitPart = filterSaUnit.As<SaUnitPart>(); 
        var contactPart = filterSaUnit.As<ContactPart>();

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var saUnitDto = new SaUnitDto
        {
            CoverUrl = saUnitPart.SaPhoto.FileId,

            Description = isLithuanian
                ? saUnitPart.DescriptionLt
                : saUnitPart.DescriptionEn,

            LinkedInUrl = saUnitPart.LinkedInUrl,
            FacebookUrl = saUnitPart.FacebookUrl,
            InstagramUrl = saUnitPart.InstagramUrl,
            Address = saUnitPart.Address,
            Email = contactPart.Email,
            PhoneNumber = contactPart.PhoneNumber,
        };

        return Ok(saUnitDto);
    }
}