using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ArticleMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(CardPart), part => part
            .Attachable()
            .WithField(nameof(CardPart.ImageUploadField), field => field
                .OfType(nameof(ImageUploadField))
                .WithDisplayName("Upload cover image"))
            .WithDescription("Card part for displaying main info")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ArticlePart), part => part
            .Attachable()
            .WithField(nameof(ArticlePart.HtmlContentLt), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Article Content LT")
                .WithPosition("6"))
            .WithField(nameof(ArticlePart.HtmlContentEn), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Article Content EN")
                .WithPosition("7"))
            .WithDescription("Articles content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Article.ToString(), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(CardPart), part => part
                .WithPosition("1"))
            .WithPart(nameof(ArticlePart), part => part
                .WithPosition("2"))
            .WithDescription("Articles content type")
        );

        return 1;
    }
}
