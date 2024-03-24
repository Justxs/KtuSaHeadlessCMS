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
                })
            .Location("Detail", "Content:10");
    }

    public override IDisplayResult Edit(EventPart part, BuildPartEditorContext context)
    {
        return Initialize<EventPartViewModel>(
            GetEditorShapeType(context), model =>
            {
                model.FbEventLink = part.FbEventLink;
            });
    }

    public override async Task<IDisplayResult> UpdateAsync(EventPart part, UpdatePartEditorContext context)
    {
        var model = new EventPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(model, Prefix))
        {
            return Edit(part, context);
        }

        part.FbEventLink = model.FbEventLink;

        return Edit(part, context);
    }
}