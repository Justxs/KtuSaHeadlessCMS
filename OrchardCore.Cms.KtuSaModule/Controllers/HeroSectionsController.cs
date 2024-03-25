using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Services;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class HeroSectionsController(IContentManager contentManager, ISession session, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string HeroSection = ContentTypeNames.HeroSection.ToString();

    [HttpGet("{sectionName}")]
    public async Task<ActionResult> GetMainContacts(string language, string sectionName)
    {
        var heroSections = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == HeroSection)
            .ListAsync();

        var filteredSection = new ContentItem();
        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        foreach (var section in heroSections)
        {
            await contentManager.LoadAsync(section);
            var part = section.As<HeroSectionPart>();

            var title = (isLithuanian
                ? part?.TitleLt
                : part?.TitleEn)!;

            if (title != sectionName)
            {
                continue;
            }

            filteredSection = section;
            break;
        }

        var heroSectionPart = filteredSection.As<HeroSectionPart>();

        var heroSectionDto = new HeroSectionDto
        {
            Title = (isLithuanian
                ? heroSectionPart?.TitleLt
                : heroSectionPart?.TitleEn)!,

            Description = (isLithuanian
                ? heroSectionPart?.DescriptionLt.Text
                : heroSectionPart?.DescriptionEn.Text)!,

            ImgSrc = heroSectionPart?.ImageUploadField.FileId!,
        };

        return Ok(heroSectionDto);
    }
}