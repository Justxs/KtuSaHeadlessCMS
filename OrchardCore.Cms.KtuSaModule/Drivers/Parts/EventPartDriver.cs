using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class EventPartDriver(IFientaService fientaService) : ContentPartDisplayDriver<EventPart>
{
    public override IDisplayResult Display(EventPart part, BuildPartDisplayContext context)
    {
        return Initialize<EventPartViewModel>(
            GetDisplayShapeType(context), model =>
            {
                model.TitleLt = part.TitleLt;
                model.TitleEn = part.TitleEn;
                model.FbEventLink = part.FbEventLink;
                model.FientaTicketLink = part.FientaTicketLink;
                model.Address = part.Address;
                model.StartDate = part.StartDate;
                model.EndDate = part.EndDate;
            })
            .Location("Detail", "Content:2");
    }

    public override async Task<IDisplayResult> EditAsync(EventPart part, BuildPartEditorContext context)
    {
        var eventsLt = await fientaService.FetchKtuSaEventsAsync("lt");
        var eventsEn = await fientaService.FetchKtuSaEventsAsync("en");

        var fientaEventOptions = eventsLt.Select(e => new SelectListItem
        {
            Text = e.Title,
            Value = e.Url,
        }).ToList();

        return Initialize<EventPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.TitleLt = part.TitleLt;
                model.TitleEn = part.TitleEn;
                model.FbEventLink = part.FbEventLink;
                model.FientaEventListLt = eventsLt;
                model.FientaEventListEn = eventsEn;
                model.FientaEventOptions = fientaEventOptions;
                model.FientaTicketLink = part.FientaTicketLink;
                model.Address = part.Address;
                model.StartDate = part.StartDate;
                model.EndDate = part.EndDate;
            })
            .Location("Content:2");
    }

    public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
    {
        var model = new EventPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return await EditAsync(part, context);
        }

        if (model.StartDate <= DateTime.Today)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".StartDate", "The event start date must be later than today's date.");

            return await EditAsync(part, context);
        }

        if (model.EndDate < model.StartDate)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".EndDate", "The event end date must be later than start date");

            return await EditAsync(part, context);
        }

        part.TitleLt = model.TitleLt;
        part.TitleEn = model.TitleEn;
        part.FbEventLink = model.FbEventLink;
        part.FientaTicketLink = model.FientaTicketLink;
        part.Address = model.Address;
        part.StartDate = model.StartDate;
        part.EndDate = model.EndDate;

        return await EditAsync(part, context);
    }
}