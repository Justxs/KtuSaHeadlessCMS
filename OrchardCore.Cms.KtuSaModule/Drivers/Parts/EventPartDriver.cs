using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class EventPartDriver(
    IFientaService fientaService,
    IContentManager contentManager,
    IResourceManager resourceManager) : ContentPartDisplayDriver<EventPart>
{
    public override async Task<IDisplayResult> DisplayAsync(EventPart part, BuildPartDisplayContext context)
    {
        var organisersField = await contentManager.GetAsync(part.OrganisersField.ContentItemIds);

        return Initialize<EventBadgeViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    if (organisersField != null)
                        model.SaUnitsDisplayNames = [.. organisersField.Select(organiser => organiser.DisplayText)];
                })
            .Location("SummaryAdmin", "Tags:11");
    }

    public override async Task<IDisplayResult> EditAsync(EventPart part, BuildPartEditorContext context)
    {
        resourceManager.RegisterResource("script", ResourceNames.FlatpickrFieldJs).AtHead();
        resourceManager.RegisterResource("stylesheet", ResourceNames.FlatpickrCss).AtHead();

        var eventsLt = await fientaService.FetchKtuSaEventsAsync("lt");
        var eventsEn = await fientaService.FetchKtuSaEventsAsync("en");

        var fientaEventOptions = eventsLt
            .Join(eventsEn,
                eventLt => eventLt.Id,
                eventEn => eventEn.Id,
                (eventLt, eventEn) => new SelectListItem
                {
                    Text = eventLt.Title,
                    Value = $"{eventLt.Url}|||{eventEn.Url}"
                })
            .ToList();

        return Initialize<EventPartViewModel>(
                GetEditorShapeType(context), model =>
                {
                    model.TitleLt = part.TitleLt;
                    model.TitleEn = part.TitleEn;
                    model.FbEventLink = part.FbEventLink;
                    model.FientaEventListLt = eventsLt;
                    model.FientaEventListEn = eventsEn;
                    model.FientaEventOptions = fientaEventOptions;
                    model.FientaTicketLinkLt = part.FientaTicketLinkLt;
                    model.FientaTicketLinkEn = part.FientaTicketLinkEn;
                    model.Address = part.Address;
                    model.StartDate = part.StartDate;
                    model.EndDate = part.EndDate;
                })
            .Location("Content:2");
    }

    public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
    {
        var model = new EventPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix)) return await EditAsync(part, context);

        if (model.StartDate <= DateTime.Today)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".StartDate",
                "The event start date must be later than today's date.");

            return await EditAsync(part, context);
        }

        if (model.EndDate < model.StartDate)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".EndDate",
                "The event end date must be later than start date");

            return await EditAsync(part, context);
        }

        part.TitleLt = model.TitleLt;
        part.TitleEn = model.TitleEn;
        part.FbEventLink = model.FbEventLink;
        part.FientaTicketLinkLt = model.FientaTicketLinkLt;
        part.FientaTicketLinkEn = model.FientaTicketLinkEn;
        part.Address = model.Address;
        part.StartDate = model.StartDate;
        part.EndDate = model.EndDate;

        return await EditAsync(part, context);
    }
}