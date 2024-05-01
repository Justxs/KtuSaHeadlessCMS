using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ActivityReportsController(IRepository repository) : ControllerBase
{
    [HttpGet("SaUnits/{saUnit}")]
    [ProducesResponseType(typeof(List<ActivityReportDto>), 200)]
    public async Task<ActionResult> GetActivityReports(string language, SaUnit saUnit)
    {
        var activityReports = await repository.GetAllAsync(ContentTypeConstants.ActivityReport);
        var saUnitItem = await repository.GetSaUnitByName(saUnit);

        var filteredSection = activityReports
            .Where(item => item.As<ActivityReportPart>().SaUnit.ContentItemIds.Contains(saUnitItem.ContentItemId))
            .OrderByDescending(item => item.As<ActivityReportPart>().From)
            .ToList();

        var isLithuanian = language.IsLtLanguage();

        var activityReportsDto = filteredSection.Select(item =>
        {
            var part = item.As<ActivityReportPart>();

            var dto = new ActivityReportDto
            {
                Id = item.ContentItemId,

                PdfUrl = isLithuanian
                    ? part.ReportLt.FileId
                    : part.ReportEn.FileId,

                From = part.From,
                To = part.To,
            };

            return dto;
        }).ToList();

        return Ok(activityReportsDto);
    }
}