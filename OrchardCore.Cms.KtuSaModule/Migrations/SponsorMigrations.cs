using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SponsorMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 7;

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
        await contentDefinitionManager.DeleteTypeDefinitionAsync(Sponsor);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(SponsorPart));
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(SponsorPart), part =>
            part.Attachable()
                .WithField(nameof(SponsorPart.Logo), field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Upload company Logo")
                    .WithSettings(new MediaFieldSettings
                    {
                        Required = true,
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".svg"]
                    }))
                .WithDescription("Sponsors content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(Sponsor, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(SponsorPart))
            .WithDescription("Sponsors content type")
        );
    }
}