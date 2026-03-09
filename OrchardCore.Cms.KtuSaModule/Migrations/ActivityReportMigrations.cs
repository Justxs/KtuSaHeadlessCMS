using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ActivityReportMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 7;

    public async Task<int> CreateAsync()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync(ActivityReport);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(ActivityReportPart));
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ActivityReportPart), part => part
            .Attachable()
            .WithField(nameof(ActivityReportPart.ReportFileLt), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload Lt version report")
                .WithSettings(new MediaFieldSettings
                {
                    Required = true,
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".pdf"]
                }))
            .WithField(nameof(ActivityReportPart.ReportFileEn), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload En version report")
                .WithSettings(new MediaFieldSettings
                {
                    Required = true,
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".pdf"]
                }))
            .WithField(nameof(ActivityReportPart.SaUnit), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select SA unit")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    DisplayedContentTypes = [SaUnit],
                    Required = true
                }))
            .WithDescription("Activity report content part"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ActivityReport, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(ActivityReportPart))
            .WithDescription("Activity report content type"));
    }
}