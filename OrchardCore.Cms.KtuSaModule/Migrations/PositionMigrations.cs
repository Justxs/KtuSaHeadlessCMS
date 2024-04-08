using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class PositionMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(PositionPart), part => part
            .Attachable()
            .WithDescription("Position info: name and description")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Position.ToString(), type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(PositionPart))
            .WithDescription("All info about member")
        );

        return 1;
    }
}