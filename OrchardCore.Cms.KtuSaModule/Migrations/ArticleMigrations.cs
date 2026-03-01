using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ArticleMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 8;
    private const string LegacyCardImageFieldName = "ImageUploadField";

    public async Task<int> CreateAsync()
    {
        await UpdateSchemaToCurrentAsync();

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom7Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    private async Task UpdateSchemaToCurrentAsync()
    {
        await AlterCardPartDefinitionAsync();
        await AlterArticlePartDefinitionAsync();
        await AlterArticleTypeDefinitionAsync();
    }

    private Task AlterCardPartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(CardPart), part => part
            .Attachable()
            .RemoveField(LegacyCardImageFieldName)
            .WithDescription("Card part for displaying main info"));
    }

    private Task AlterArticlePartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(ArticlePart), part => part
            .Attachable()
            .WithField(nameof(ArticlePart.ThumbnailImage), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Hero Image")
                .WithPosition("0")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithField(nameof(ArticlePart.HtmlContentLt), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("HTML Content LT")
                .WithPosition("2"))
            .WithField(nameof(ArticlePart.HtmlContentEn), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("HTML Content EN")
                .WithPosition("3"))
            .WithDescription("Articles content part"));
    }

    private Task AlterArticleTypeDefinitionAsync()
    {
        return contentDefinitionManager.AlterTypeDefinitionAsync(Article, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(CardPart), part => part.WithPosition("1"))
            .WithPart(nameof(ArticlePart), part => part.WithPosition("2"))
            .WithDescription("Articles content type"));
    }
}
