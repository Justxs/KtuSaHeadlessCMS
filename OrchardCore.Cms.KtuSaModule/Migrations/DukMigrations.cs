using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class DukMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(DukPart), part => 
            part.Attachable());

        await contentDefinitionManager.AlterTypeDefinitionAsync("Duk", type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(DukPart))
        );

        return 1;
    }
}