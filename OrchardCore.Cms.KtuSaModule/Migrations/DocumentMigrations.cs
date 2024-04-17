using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class DocumentMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(CategoryPart), part =>
            part.Attachable()
                .WithDescription("Content type category part")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(DocumentPart), part =>
            part.Attachable()
                .WithField(nameof(DocumentPart.DocumentLt), field => field
                    .OfType(nameof(PdfUploadField))
                    .WithDisplayName("Upload Lt version document"))
                .WithField(nameof(DocumentPart.DocumentEn), field => field
                    .OfType(nameof(PdfUploadField))
                    .WithDisplayName("Upload En version document"))
                .WithField(nameof(DocumentPart.CategoryField), field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Select document category")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = false,
                        DisplayedContentTypes = [DocumentCategory],
                    }))
                .WithDescription("Document content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(DocumentCategory, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(CategoryPart))
            .WithDescription("Document category content type")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(Document, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(DocumentPart))
            .WithDescription("Document content type")
        );

        return 1;
    }
}