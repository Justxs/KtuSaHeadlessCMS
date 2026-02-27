using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class StaticPagesController(IRepository repository) : ControllerBase
{
    [HttpGet("{pageName}")]
    [ProducesResponseType(typeof(StaticPageDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<ActionResult> GetHeroSection(string language, string pageName)
    {
        var staticPages = await repository.GetAllAsync(StaticPage);

        var isLithuanian = language.IsLtLanguage();

        var filteredSection = staticPages
            .FirstOrDefault(page => page.DisplayText.Contains(pageName));

        if (filteredSection == null) return NotFound("Page not found");

        var staticPagePart = filteredSection.As<StaticPagePart>();

        var staticPageDto = new StaticPageDto
        {
            Body = isLithuanian
                ? staticPagePart.BodyLt.HtmlBody
                : staticPagePart.BodyEn.HtmlBody
        };

        return Ok(staticPageDto);
    }
}