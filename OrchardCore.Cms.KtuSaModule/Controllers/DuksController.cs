using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class DuksController(IRepository repository, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string DukContentType = ContentTypeNames.Duk.ToString();

    [HttpGet]
    [ProducesResponseType(typeof(List<DukDto>), 200)]
    public async Task<ActionResult> GetDuks(string language, [FromQuery] int? limit)
    {
        var duks = await repository.GetAllAsync(DukContentType);

        duks = duks.OrderByDescending(item => item.ModifiedUtc);

        if (limit is not null)
        {
            duks = duks.OrderBy(_ => Guid.NewGuid())
                .Take((int)limit)
                .ToList();
        }


        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var dukDtos = duks.Select(item =>
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

        return Ok(dukDtos);
    }
}