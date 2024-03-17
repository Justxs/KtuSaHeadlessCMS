using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ArticleMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ArticlePart), part => part
            .Attachable()
            .WithField(nameof(ArticlePart.HtmlContentLt), field => field
                .OfType(nameof(HtmlField))
                .WithDisplayName("LT Article Content")
                .WithEditor("Wysiwyg"))
            .WithField(nameof(ArticlePart.HtmlContentEn), field => field
                .OfType(nameof(HtmlField))
                .WithDisplayName("EN Article Content")
                .WithEditor("Wysiwyg"))
            .WithField(nameof(ArticlePart.ImageUploadField), field => field
                .OfType(nameof(ArticlePart.ImageUploadField))
                .WithDisplayName("Upload main image"))
            .WithDescription("Articles content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Article.ToString(), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(ArticlePart))
            .WithDescription("Articles content type")
        );

        return 1;
    }
}
