using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Extensions;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class HeroSectionsController(IRepository repository) : ControllerBase
{
    [HttpGet("{sectionName}")]
    [ProducesResponseType(typeof(HeroSectionDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<ActionResult> GetHeroSection(string language, string sectionName)
    {
        var heroSections = await repository.GetAllAsync(HeroSection);

        var isLithuanian = language.IsLtLanguage();

        var filteredSection = heroSections
            .Select(section => new
            {
                Section = section, 
                Part = section.As<HeroSectionPart>(),
            })
            .FirstOrDefault(x => (isLithuanian
                ? x.Part?.TitleLt
                : x.Part?.TitleEn).Contains(sectionName, StringComparison.CurrentCultureIgnoreCase))
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