using System.Text.Json.Nodes;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Lists.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using YesSql;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class DocumentMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    ISession session) : DataMigration
{
    private const int CurrentVersion = 8;

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
        return 6;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(DocumentPart), part =>
            part.RemoveField("CategoryField"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.Document, type => type
            .Draftable()
            .Creatable(false)
            .Listable(false)
            .WithPart(nameof(DocumentPart))
            .WithDescription("Document content type"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.DocumentCategory, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(CategoryPart))
            .WithPart("ListPart", part => part
                .MergeSettings<ListPartSettings>(s =>
                {
                    s.ContainedContentTypes = [ContentTypeConstants.Document];
                    s.EnableOrdering = true;
                }))
            .WithDescription("Document category content type"));

        var categories = await session
            .Query<ContentItem, ContentItemIndex>(i =>
                i.ContentType == ContentTypeConstants.DocumentCategory && i.Published)
            .ListAsync();

        var documents = await session
            .Query<ContentItem, ContentItemIndex>(i => i.ContentType == ContentTypeConstants.Document && i.Published)
            .ListAsync();

        foreach (var category in categories)
        {
            var order = 0;
            var categoryDocuments = documents
                .Where(d =>
                {
                    var json = (JsonObject?)d.Content[nameof(DocumentPart)];
                    var ids = json?["CategoryField"]?["ContentItemIds"]?.AsArray();
                    return ids is not null && ids
                        .Select(n => n?.GetValue<string>())
                        .Contains(category.ContentItemId);
                });

            foreach (var doc in categoryDocuments)
            {
                doc.Alter<ContainedPart>(part =>
                {
                    part.ListContentItemId = category.ContentItemId;
                    part.Order = order++;
                });

                await contentManager.UpdateAsync(doc);
            }
        }

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom7Async()
    {
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
                        Required = true,
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".pdf"]
                    }))
                .WithField(nameof(DocumentPart.FileEn), field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Upload En version document")
                    .WithSettings(new MediaFieldSettings
                    {
                        Required = true,
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".pdf"]
                    }))
                .WithDescription("Document content part"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.DocumentCategory, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(CategoryPart))
            .WithPart("ListPart", part => part
                .MergeSettings<ListPartSettings>(s =>
                {
                    s.ContainedContentTypes = [ContentTypeConstants.Document];
                    s.EnableOrdering = true;
                }))
            .WithDescription("Document category content type"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.Document, type => type
            .Draftable()
            .Creatable(false)
            .Listable(false)
            .WithPart(nameof(DocumentPart))
            .WithDescription("Document content type"));
    }
}