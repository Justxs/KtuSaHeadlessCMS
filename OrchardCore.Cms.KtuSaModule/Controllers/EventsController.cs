using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using YesSql;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class EventsController(IContentManager contentManager, ISession session, IStringActionService stringActionService) : ControllerBase
{
    // TODO add indexes for event type
    // TODO add sa unit filtering
    private static readonly string EventContentType = ContentTypeNames.Event.ToString();

    [HttpGet]
    public async Task<ActionResult> GetArticles(string language)
    {
        var events = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == EventContentType && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();

        var filteredSection = new List<ContentItem>();

        foreach (var eventItem in events)
        {
            await contentManager.LoadAsync(eventItem);

            var part = eventItem.As<EventPart>();

            if (part != null && part.Date > DateTime.Now)
            {
                filteredSection.Add(eventItem);
            }
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var eventDtos = filteredSection.Select(item =>
        {
            var part = item.As<EventPart>();
            var cardPart = item.As<CardPart>();

            var dto = new EventDto
            {
                Title = isLithuanian
                    ? cardPart.TitleLt
                    : cardPart.TitleEn,

                Preview = isLithuanian
                    ? cardPart.PreviewLt
                    : cardPart.PreviewEn,

                Id = item.ContentItemId,
                Date = part.Date,
                ThumbnailImageId = cardPart.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(eventDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetArticleById(string language, string id)
    {
        var eventItem = await contentManager.GetAsync(id);

        await contentManager.LoadAsync(eventItem);

        if (!eventItem.Published)
        {
            return BadRequest("Event is not published yet.");
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var cardPart = eventItem.As<CardPart>();
        var eventPart = eventItem.As<EventPart>();

        var eventDto = new EventDto
        {
            Title = isLithuanian
                ? cardPart.TitleLt
                : cardPart.TitleEn,

            Preview = isLithuanian
                ? cardPart.PreviewLt
                : cardPart.PreviewEn,

            HtmlBody = isLithuanian
                ? eventPart.BodyFieldLt.HtmlBody
                : eventPart.BodyFieldEn.HtmlBody,

            Id = eventItem.ContentItemId,
            Date = eventPart.Date,
            ThumbnailImageId = cardPart.ImageUploadField.FileId,
        };

        return Ok(eventDto);
    }
}