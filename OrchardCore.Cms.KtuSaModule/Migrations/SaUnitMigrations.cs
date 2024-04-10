using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SaUnitMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(SaUnitPart), part => part
            .Attachable()
            .WithField(nameof(SaUnitPart.SaPhoto), field => field
                .OfType(nameof(ImageUploadField))
                .WithDisplayName("Upload FSA photo"))
            .WithDescription("Sa unit content part"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(nameof(ContentTypeNames.SaUnit), type => type
            .Listable()
            .WithPart(nameof(SaUnitPart))
            .WithPart(nameof(ContactPart))
            .WithDescription("Sa unit content type"));

        foreach (var saUnit in (SaUnit[])Enum.GetValues(typeof(SaUnit)))
        {
            await CreateSaUnitsAsync(saUnit);
        }

        return 1;
    }

    private async Task CreateSaUnitsAsync(SaUnit saUnit)
    {
        if (saUnit is SaUnit.BRK or SaUnit.CSA)
        {
            return;
        }

        var saUnitItem = await contentManager.NewAsync(nameof(ContentTypeNames.SaUnit));
        saUnitItem.DisplayText = saUnit.ToString().Replace("_", " ");

        var saUnitPart = saUnitItem.As<SaUnitPart>();

        saUnitPart.UnitName = saUnit.ToString();

        saUnitItem.Apply(nameof(SaUnitPart), saUnitPart);

        await contentManager.CreateAsync(saUnitItem);
    }
}