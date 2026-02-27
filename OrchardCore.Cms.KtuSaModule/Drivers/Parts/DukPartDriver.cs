using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.ViewModels.Parts;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Parts;

public class DukPartDriver : ContentPartDisplayDriver<DukPart>
{
    public override IDisplayResult Display(DukPart part, BuildPartDisplayContext context)
    {
        return Initialize<DukPartViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(DukPart part, BuildPartEditorContext context)
    {
        var test = GetEditorShapeType(context);
        return Initialize<DukPartViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:5");
    }

    public override async Task<IDisplayResult> UpdateAsync(DukPart part, UpdatePartEditorContext context)
    {
        var viewModel = new DukPartViewModel();

        if (!await context.Updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            return await EditAsync(part, context);
        }

        part.QuestionLt = viewModel.QuestionLt;
        part.QuestionEn = viewModel.QuestionEn;
        part.AnswerLt = viewModel.AnswerLt;
        part.AnswerEn = viewModel.AnswerEn;

        return await EditAsync(part, context);
    }

    private static void PopulateViewModel(DukPart part, DukPartViewModel viewModel)
    {
        viewModel.QuestionLt = part.QuestionLt;
        viewModel.QuestionEn = part.QuestionEn;
        viewModel.AnswerLt = part.AnswerLt;
        viewModel.AnswerEn = part.AnswerEn;
    }
}