using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule.Drivers;

public class QuillFieldDriver(IResourceManager resourceManager) : ContentFieldDisplayDriver<QuillField>
{
    public override IDisplayResult Display(QuillField field, BuildFieldDisplayContext context)
    {
        return Initialize<QuillFieldViewModel>(
                GetDisplayShapeType(context),
                viewModel =>
                {
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                    viewModel.HtmlBody = field.HtmlBody;
                })
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(QuillField field, BuildFieldEditorContext context)
    {
        var settings = resourceManager.RegisterResource("script", "QuillJs");
        settings.AtHead();

        settings = resourceManager.RegisterResource("stylesheet", "QuillCss");
        settings.AtHead();

        return Initialize<QuillFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                    viewModel.HtmlBody = field.HtmlBody;
                })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(QuillField field, IUpdateModel updater, UpdateFieldEditorContext context)
    {
        var viewModel = new QuillFieldViewModel();

        await updater.TryUpdateModelAsync(viewModel, Prefix);

        field.HtmlBody = viewModel.HtmlBody;

        return await EditAsync(field, context);
    }
}