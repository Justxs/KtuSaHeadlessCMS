using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class CardPartDriver : ContentPartDisplayDriver<CardPart>
{
    public override IDisplayResult Display(CardPart part, BuildPartDisplayContext context)
    {
        return Initialize<CardPartViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Detail", "Content:1")
            .Location("Summary", "Content:1");
    }

    public override IDisplayResult Edit(CardPart part, BuildPartEditorContext context)
    {
        var test = GetEditorShapeType(context);
        return Initialize<CardPartViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:1");
            
    }

    public override async Task<IDisplayResult> UpdateAsync(CardPart part, IUpdateModel updater, UpdatePartEditorContext context)
    {
        var viewModel = new CardPartViewModel();

        if (!await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            return await EditAsync(part, context);
        }

        part.TitleLt = viewModel.TitleLt;
        part.TitleEn = viewModel.TitleEn;
        part.PreviewLt = viewModel.PreviewLt;
        part.PreviewEn = viewModel.PreviewEn;

        return await EditAsync(part, context);
    }

    private static void PopulateViewModel(CardPart part, CardPartViewModel viewModel)
    {
        viewModel.TitleLt = part.TitleLt;
        viewModel.TitleEn = part.TitleEn;
        viewModel.PreviewLt = part.PreviewLt;
        viewModel.PreviewEn = part.PreviewEn;
    }
}