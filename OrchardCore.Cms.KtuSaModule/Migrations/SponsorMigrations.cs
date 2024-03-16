using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SponsorMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(SponsorPart), part => 
            part.Attachable()
                .WithField(nameof(ArticlePart.ImageUploadField), field => field
                    .OfType(nameof(ArticlePart.ImageUploadField))
                    .WithDisplayName("Upload company Logo")));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Sponsor.ToString(), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(SponsorPart))
        );

        return 1;
    }
}