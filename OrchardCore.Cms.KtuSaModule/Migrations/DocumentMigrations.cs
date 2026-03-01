using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class DocumentMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 6;

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
        await contentDefinitionManager.DeleteTypeDefinitionAsync(ContentTypeConstants.Document);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(DocumentPart));
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(CategoryPart), part =>
            part.Attachable()
                .WithDescription("Content type category part"));

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(DocumentPart), part =>
            part.Attachable()
                .WithField(nameof(DocumentPart.FileLt), field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Upload Lt version document")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".pdf"]
                    }))
                .WithField(nameof(DocumentPart.FileEn), field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Upload En version document")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".pdf"]
                    }))
                .WithField(nameof(DocumentPart.CategoryField), field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Select document category")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = false,
                        DisplayedContentTypes = [ContentTypeConstants.DocumentCategory],
                        Required = true
                    }))
                .WithDescription("Document content part"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.DocumentCategory, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(CategoryPart))
            .WithDescription("Document category content type"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.Document, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(DocumentPart))
            .WithDescription("Document content type"));
    }
}
