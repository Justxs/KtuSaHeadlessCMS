using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class DukMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(DukPart), part => 
            part
                .Attachable()
                .WithDescription("Frequently asked questions content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Duk.ToString(), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(DukPart))
            .WithDescription("Frequently asked questions content type")
        );

        return 1;
    }
}