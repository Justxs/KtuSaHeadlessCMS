using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SaUnitMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager)
    : DataMigration
{
    private const int CurrentVersion = 6;

    public async Task<int> CreateAsync()
    {
        await ApplySchemaAsync();

        foreach (var saUnit in Enum.GetValues<SaUnit>())
        {
            await CreateSaUnitsAsync(saUnit);
        }

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
        await contentDefinitionManager.DeleteTypeDefinitionAsync(ContentTypeConstants.SaUnit);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(SaUnitPart));
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(SaUnitPart), part => part
            .Attachable()
            .WithField(nameof(SaUnitPart.UnitPhoto), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload FSA photo")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithDescription("Sa unit content part"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.SaUnit, type => type
            .Listable()
            .WithPart(nameof(SaUnitPart))
            .WithPart(nameof(ContactPart))
            .WithDescription("Sa unit content type"));
    }

    private async Task CreateSaUnitsAsync(SaUnit saUnit)
    {
        var saUnitItem = await contentManager.NewAsync(ContentTypeConstants.SaUnit);
        saUnitItem.DisplayText = saUnit.ToString().Replace("_", " ");

        var saUnitPart = saUnitItem.As<SaUnitPart>();

        saUnitPart.UnitName = saUnit.ToString();

        saUnitItem.Apply(nameof(SaUnitPart), saUnitPart);

        await contentManager.CreateAsync(saUnitItem);
    }
}
