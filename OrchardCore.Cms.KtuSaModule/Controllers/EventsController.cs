using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Services;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class EventsController(IContentManager contentManager, IStringActionService stringActionService, IRepository repository) : ControllerBase
{
    // TODO add indexes for event type
    // TODO add sa unit filtering
    private static readonly string EventContentType = ContentTypeNames.Event.ToString();
    private static readonly string SaUnitContentType = ContentTypeNames.SaUnit.ToString();


    [HttpGet]
    public async Task<ActionResult> GetArticles(string language)
    {
        var events = await repository.GetAllAsync(EventContentType);

        var filteredSection = new List<ContentItem>();

        foreach (var eventItem in events)
        {
            await contentManager.LoadAsync(eventItem);

            var part = eventItem.As<EventPart>();

            if (part != null && part.StartDate > DateTime.Now)
            {
                filteredSection.Add(eventItem);
            }
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var eventDtos = filteredSection.Select(item =>
        {
            var part = item.As<EventPart>();

            var dto = new EventDto
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
    public async Task<ActionResult> GetArticleById(string language, string id)
    {
        var eventItem = await contentManager.GetAsync(id);

        if (eventItem is null)
        {
            return NotFound("Article not found");
        }

        if (!eventItem.Published)
        {
            return BadRequest("Event is not published yet.");
        }

        await contentManager.LoadAsync(eventItem);
        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var saUnits = await repository.GetAllAsync(SaUnitContentType);

        var part = eventItem.As<EventPart>();

        var saUnitIds = part.OrganisersField.ContentItemIds;

        var organisers = saUnits
            .Where(unit => saUnitIds.Contains(unit.ContentItemId))
            .Select(unit => unit.As<SaUnitPart>().UnitName)
            .ToList();

        var eventDto = new EventDto
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