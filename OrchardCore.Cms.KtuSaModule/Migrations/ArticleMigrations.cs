using OrchardCore.Cms.KtuSaModule.Models;
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
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync("Article", type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(ArticlePart))
        );

        return 1;
    }
}
