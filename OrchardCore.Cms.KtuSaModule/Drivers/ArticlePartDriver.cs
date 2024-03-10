using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class ArticlePartDriver : ContentPartDisplayDriver<ArticlePart>
{
    public override IDisplayResult Display(ArticlePart part, BuildPartDisplayContext context)
    {
        return Initialize<ArticlePartViewModel>(
                GetDisplayShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Detail", "Content:5")
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(ArticlePart part, BuildPartEditorContext context)
    {
        var test = GetEditorShapeType(context);
        return Initialize<ArticlePartViewModel>(
                GetEditorShapeType(context),
                viewModel => PopulateViewModel(part, viewModel))
            .Location("Content:5");
    }

    public override async Task<IDisplayResult> UpdateAsync(ArticlePart part, IUpdateModel updater, UpdatePartEditorContext context)
    {
        var viewModel = new ArticlePartViewModel();

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

    private static void PopulateViewModel(ArticlePart part, ArticlePartViewModel viewModel)
    {
        viewModel.TitleLt = part.TitleLt;
        viewModel.TitleEn = part.TitleEn;
        viewModel.PreviewLt = part.PreviewLt;
        viewModel.PreviewEn = part.PreviewEn;
    }
}