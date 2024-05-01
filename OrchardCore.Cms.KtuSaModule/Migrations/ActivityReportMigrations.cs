using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ActivityReportMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ActivityReportPart), part =>
            part.Attachable()
                .WithField(nameof(ActivityReportPart.ReportLt), field => field
                    .OfType(nameof(PdfUploadField))
                    .WithDisplayName("Upload Lt version report"))
                .WithField(nameof(ActivityReportPart.ReportEn), field => field
                    .OfType(nameof(PdfUploadField))
                    .WithDisplayName("Upload En version report"))
                .WithField(nameof(ActivityReportPart.SaUnit), field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Select SA unit")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = false,
                        DisplayedContentTypes = [SaUnit],
                        Required = true,
                    }))
            .WithDescription("Activity report content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ActivityReport, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(ActivityReportPart))
        .WithDescription("Activity report content type")
        );

        return 1;
    }
}