using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos.Events;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Extensions;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class EventsController(
    IContentManager contentManager, 
    IRepository repository) : ControllerBase
{
    // TODO add indexes for event type
    private static readonly string EventContentType = ContentTypeNames.Event.ToString();
    private static readonly string SaUnitContentType = ContentTypeNames.SaUnit.ToString();

    [HttpGet]
    [ProducesResponseType(typeof(List<EventPreviewDto>), 200)]
    public async Task<ActionResult> GetEvents(string language, [FromQuery] bool fetchPassed)
    {
        var events = await repository.GetAllAsync(EventContentType);

        var filteredSection = events
            .Where(eventItem => fetchPassed || eventItem.As<EventPart>().StartDate > DateTime.Now)
            .OrderByDescending(item => item.As<EventPart>().StartDate)
            .ToList();

        var isLithuanian =language.IsLtLanguage();

        var eventDtos = filteredSection.Select(item =>
        {
            var part = item.As<EventPart>();

            var dto = new EventPreviewDto
            {
                Id = item.ContentItemId,

                Title = isLithuanian
                    ? part.TitleLt
                    : part.TitleEn,

                StartDate = part.StartDate,
                CoverImageUrl = part.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(eventDtos);
    }

    [HttpGet("SaUnits/{saUnit}")]
    [ProducesResponseType(typeof(List<EventPreviewDto>), 200)]
    public async Task<ActionResult> GetEventsByFsa(string language, SaUnit saUnit, [FromQuery] bool fetchPassed)
    {
        var events = await repository.GetAllAsync(EventContentType);
        var saUnits = await repository.GetAllAsync(SaUnitContentType);
        var filterSaUnit = saUnits.FirstOrDefault(item => item.As<SaUnitPart>().UnitName == saUnit.ToString());

        var filteredSection = events
            .Where(eventItem => fetchPassed || eventItem.As<EventPart>().StartDate >= DateTime.Now)
            .Where(eventItem => eventItem.As<EventPart>().OrganisersField.ContentItemIds.Contains(filterSaUnit!.ContentItemId))
            .OrderByDescending(item => item.As<EventPart>().StartDate)
            .ToList();

        var isLithuanian = language.IsLtLanguage();

        var eventDtos = filteredSection.Select(item =>
        {
            var part = item.As<EventPart>();

            var dto = new EventPreviewDto
            {
                Id = item.ContentItemId,

                Title = isLithuanian
                    ? part.TitleLt
                    : part.TitleEn,

                StartDate = part.StartDate,
                CoverImageUrl = part.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(eventDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventContentDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<ActionResult> GetEventById(string language, string id)
    {
        var eventItem = await contentManager.GetAsync(id);

        if (eventItem is null)
        {
            return NotFound("Article not found");
        }

        var isLithuanian = language.IsLtLanguage();

        var part = eventItem.As<EventPart>();

        var saUnits = await contentManager.GetAsync(part.OrganisersField.ContentItemIds);

        var organisers = saUnits
            .Select(unit => unit.As<SaUnitPart>().UnitName)
            .ToList();

        var eventDto = new EventContentDto
        {
            Id = eventItem.ContentItemId,

            Title = isLithuanian
                ? part.TitleLt
                : part.TitleEn,

            HtmlBody = isLithuanian
                ? part.BodyFieldLt.HtmlBody
                : part.BodyFieldEn.HtmlBody,
            
            FientaTicketUrl = isLithuanian
                ? part.FientaTicketLinkLt
                : part.FientaTicketLinkEn,

            StartDate = part.StartDate,
            EndDate = part.EndDate,
            Address = part.Address,
            FacebookUrl = part.FbEventLink,
            CoverImageUrl = part.ImageUploadField.FileId,
            Organisers = organisers,
        };

        return Ok(eventDto);
    }
}