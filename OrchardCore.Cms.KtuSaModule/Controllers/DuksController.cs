using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class DuksController(IContentManager contentManager, ISession session) : ControllerBase
{
    private static readonly string DukContentType = ContentTypeNames.Duk.ToString();

    [HttpGet]
    public async Task<ActionResult> GetDuks(string language, [FromQuery] int? limit)
    {
        var duks = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == DukContentType && index.Published)
            .OrderByDescending(index => index.ModifiedUtc)
            .ListAsync();

        if (limit is not null)
        {
            duks = duks.OrderBy(_ => Guid.NewGuid()).Take((int)limit).ToList();
        }

        foreach (var duk in duks)
        {
            await contentManager.LoadAsync(duk);
        }

        var isLithuanian = language.ToUpper() == Languages.LT.ToString();

        var articleDtos = duks.Select(item =>
        {
            var part = item.As<DukPart>();
            var dto = new DukDto
            {
                Question = (isLithuanian
                    ? part?.QuestionLt
                    : part?.QuestionEn)!,

                Answer = (isLithuanian
                    ? part?.AnswerLt
                    : part?.AnswerEn)!,

                Id = item.ContentItemId,
                ModifiedDate = (DateTime)item.ModifiedUtc!,
            };

            return dto;
        }).ToList();

        return Ok(articleDtos);
    }
}