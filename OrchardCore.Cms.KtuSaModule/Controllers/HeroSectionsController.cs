using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class HeroSectionsController(IRepository repository, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string HeroSection = ContentTypeNames.HeroSection.ToString();

    [HttpGet("{sectionName}")]
    [ProducesResponseType(typeof(HeroSectionDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<ActionResult> GetMainContacts(string language, string sectionName)
    {
        var heroSections = await repository.GetAllAsync(HeroSection);

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var filteredSection = heroSections
            .Select(section => new
            {
                Section = section, 
                Part = section.As<HeroSectionPart>(),
            })
            .FirstOrDefault(x => (isLithuanian ? x.Part?.TitleLt : x.Part?.TitleEn) == sectionName)
            ?.Section;

        if (filteredSection == null)
        {
            return NotFound("Hero section not found");
        }

        var heroSectionPart = filteredSection.As<HeroSectionPart>();

        var heroSectionDto = new HeroSectionDto
        {
            Title = isLithuanian
                ? heroSectionPart.TitleLt
                : heroSectionPart.TitleEn,

            Description = isLithuanian
                ? heroSectionPart.DescriptionLt.Text
                : heroSectionPart.DescriptionEn.Text,

            ImgSrc = heroSectionPart.ImageUploadField.FileId,
        };

        return Ok(heroSectionDto);
    }
}