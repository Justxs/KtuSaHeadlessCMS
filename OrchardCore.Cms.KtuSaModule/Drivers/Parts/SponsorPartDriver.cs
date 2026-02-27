using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class SponsorPartDriver : ContentPartDisplayDriver<SponsorPart>
{
    public override IDisplayResult Display(SponsorPart part, BuildPartDisplayContext context)
    {
        return Initialize<SponsorPartViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(SponsorPart part, BuildPartEditorContext context)
    {
        var test = GetEditorShapeType(context);
        return Initialize<SponsorPartViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:5");
    }

    public override async Task<IDisplayResult> UpdateAsync(SponsorPart part, UpdatePartEditorContext context)
    {
        var viewModel = new SponsorPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(viewModel, Prefix)) return await EditAsync(part, context);

        part.Name = viewModel.Name;
        part.WebsiteUrl = viewModel.WebsiteUrl;

        return await EditAsync(part, context);
    }

    private static void PopulateViewModel(SponsorPart part, SponsorPartViewModel viewModel)
    {
        viewModel.Name = part.Name;
        viewModel.WebsiteUrl = part.WebsiteUrl;
    }
}