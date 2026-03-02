using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.AdminControllers;

[Admin]
public class FientaAdminController(
    IFientaService fientaService,
    IContentManager contentManager,
    IMediaFileStore mediaFileStore,
    IHttpClientFactory httpClientFactory) : Controller
{
    [HttpPost]
    [Admin("Fienta/Import")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportEvent(string? contentItemId, int fientaEventId)
    {
        if (fientaEventId <= 0)
            return BadRequest();

        var isNew = string.IsNullOrEmpty(contentItemId);

        ContentItem contentItem;
        if (isNew)
        {
            contentItem = await contentManager.NewAsync(Event);
        }
        else
        {
            contentItem = await contentManager.GetAsync(contentItemId, VersionOptions.DraftRequired);
            if (contentItem == null)
                return NotFound();
        }

        var eventsLt = await fientaService.FetchKtuSaEventsAsync("lt");
        var eventsEn = await fientaService.FetchKtuSaEventsAsync("en");

        var eventLt = eventsLt.FirstOrDefault(e => e.Id == fientaEventId);
        var eventEn = eventsEn.FirstOrDefault(e => e.Id == fientaEventId);

        if (eventLt == null)
            return BadRequest("Fienta event not found");

        var eventPart = contentItem.As<EventPart>();

        eventPart.TitleLt = eventLt.Title;
        eventPart.TitleEn = eventEn?.Title ?? eventLt.Title;
        eventPart.Address = eventLt.Address;
        eventPart.FientaTicketLinkLt = eventLt.Url;
        eventPart.FientaTicketLinkEn = eventEn?.Url;

        if (DateTime.TryParse(eventLt.StartsAt, out var startDate))
            eventPart.StartDate = startDate;
        if (DateTime.TryParse(eventLt.EndsAt, out var endDate))
            eventPart.EndDate = endDate;

        if (!string.IsNullOrEmpty(eventLt.ImageUrl))
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();
                using var imageResponse = await httpClient.GetAsync(eventLt.ImageUrl);

                if (imageResponse.IsSuccessStatusCode)
                {
                    await using var imageStream = await imageResponse.Content.ReadAsStreamAsync();
                    var extension = GetImageExtension(eventLt.ImageUrl, imageResponse);
                    var mediaPath = $"events/fienta-{fientaEventId}{extension}";

                    await mediaFileStore.CreateFileFromStreamAsync(mediaPath, imageStream, true);
                    eventPart.CoverImage.Paths = [mediaPath];
                }
            }
            catch
            {
                // Image download failed, continue without image
            }
        }

        contentItem.Apply(eventPart);

        SetFlowPartDescription(contentItem, "ContentLt", eventLt.Description);
        if (eventEn != null)
            SetFlowPartDescription(contentItem, "ContentEn", eventEn.Description);

        if (isNew)
            await contentManager.CreateAsync(contentItem, VersionOptions.Draft);
        else
            await contentManager.UpdateAsync(contentItem);

        return Ok(new { contentItemId = contentItem.ContentItemId });
    }

    private static void SetFlowPartDescription(ContentItem contentItem, string flowPartName, string description)
    {
        var flowPart = contentItem.Get<FlowPart>(flowPartName);
        if (flowPart?.Widgets is not { Count: > 0 }) return;

        var widget = flowPart.Widgets[0];
        var part = widget.As<ParagraphWidgetPart>();
        if (part == null) return;

        part.Body.Html = description;
        widget.Apply(part);
        contentItem.Apply(flowPartName, flowPart);
    }

    private static string GetImageExtension(string imageUrl, HttpResponseMessage response)
    {
        var extension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
        if (!string.IsNullOrEmpty(extension)) return extension;

        return response.Content.Headers.ContentType?.MediaType switch
        {
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".jpg"
        };
    }
}
