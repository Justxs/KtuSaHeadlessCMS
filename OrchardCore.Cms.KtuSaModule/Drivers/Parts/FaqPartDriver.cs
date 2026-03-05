using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class FaqPartDriver : ContentPartDisplayDriver<FaqPart>
{
    public override IDisplayResult Display(FaqPart part, BuildPartDisplayContext context)
    {
        return Initialize<FaqPartViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(FaqPart part, BuildPartEditorContext context)
    {
        return Initialize<FaqPartViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:5");
    }

    public override async Task<IDisplayResult> UpdateAsync(FaqPart part, UpdatePartEditorContext context)
    {
        var viewModel = new FaqPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(viewModel, Prefix)) return await EditAsync(part, context);

        part.QuestionLt = viewModel.QuestionLt;
        part.QuestionEn = viewModel.QuestionEn;

        return await EditAsync(part, context);
    }

    private static void PopulateViewModel(FaqPart part, FaqPartViewModel viewModel)
    {
        viewModel.QuestionLt = part.QuestionLt;
        viewModel.QuestionEn = part.QuestionEn;
    }
}
