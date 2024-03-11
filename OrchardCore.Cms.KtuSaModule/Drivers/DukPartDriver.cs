using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class DukPartDriver : ContentPartDisplayDriver<DukPart>
{
    public override IDisplayResult Display(DukPart part, BuildPartDisplayContext context)
    {
        return Initialize<DukViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(DukPart part, BuildPartEditorContext context)
    {
        var test = GetEditorShapeType(context);
        return Initialize<DukViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:5");
    }

    public override async Task<IDisplayResult> UpdateAsync(DukPart part, IUpdateModel updater, UpdatePartEditorContext context)
    {
        var viewModel = new DukViewModel();

        if (!await updater.TryUpdateModelAsync(viewModel, Prefix))
        {
            return await EditAsync(part, context);
        }

        part.QuestionLt = viewModel.QuestionLt;
        part.QuestionEn = viewModel.QuestionEn;
        part.AnswerLt = viewModel.AnswerLt;
        part.AnswerEn = viewModel.AnswerEn;

        return await EditAsync(part, context);
    }

    private static void PopulateViewModel(DukPart part, DukViewModel viewModel)
    {
        viewModel.QuestionLt = part.QuestionLt;
        viewModel.QuestionEn = part.QuestionEn;
        viewModel.AnswerLt = part.AnswerLt;
        viewModel.AnswerEn = part.AnswerEn;
    }
}