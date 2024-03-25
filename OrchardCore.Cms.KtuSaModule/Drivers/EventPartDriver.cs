using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class EventPartDriver : ContentPartDisplayDriver<EventPart>
{
    public override IDisplayResult Display(EventPart part, BuildPartDisplayContext context)
    {
        return Initialize<EventPartViewModel>(
                GetDisplayShapeType(context), model =>
                {
                    model.FbEventLink = part.FbEventLink;
                    model.Date = part.Date;
                })
            .Location("Detail", "Content:2");
    }

    public override IDisplayResult Edit(EventPart part, BuildPartEditorContext context)
    {
        return Initialize<EventPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.FbEventLink = part.FbEventLink;
                model.Date = part.Date;
            })
            .Location("Content:2");
    }

    public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
    {
        var model = new EventPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        if (model.Date <= DateTime.Today)
        {
            context.Updater.ModelState.AddModelError(Prefix + ".Date", "The event date must be later than today's date.");

            return Edit(part, context);
        }

        part.FbEventLink = model.FbEventLink;
        part.Date = model.Date;

        return Edit(part, context);
    }
}