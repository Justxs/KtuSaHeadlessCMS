using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule.Drivers.Fields;

public class QuillFieldDriver(IResourceManager resourceManager) : ContentFieldDisplayDriver<QuillField>
{
    public override IDisplayResult Display(QuillField field, BuildFieldDisplayContext fieldDisplayContext)
    {
        return Initialize<QuillFieldViewModel>(
                GetDisplayShapeType(fieldDisplayContext),
                viewModel =>
                {
                    viewModel.Label = fieldDisplayContext.PartFieldDefinition.DisplayName();
                    viewModel.HtmlBody = field.HtmlBody;
                })
            .Location("Summary", "Content:5");
    }

    public override IDisplayResult Edit(QuillField field, BuildFieldEditorContext context)
    {
        resourceManager.RegisterResource("script", ResourceNames.QuillFieldJs).AtHead();
        resourceManager.RegisterResource("stylesheet", ResourceNames.QuillFieldCss).AtHead();

        return Initialize<QuillFieldViewModel>(
                GetEditorShapeType(context),
                viewModel =>
                {
                    viewModel.Label = context.PartFieldDefinition.DisplayName();
                    viewModel.HtmlBody = field.HtmlBody;
                })
            .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(QuillField field, UpdateFieldEditorContext context)
    {
        var viewModel = new QuillFieldViewModel();

        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        field.HtmlBody = viewModel.HtmlBody.AddH1Id();

        return await EditAsync(field, context);
    }
}