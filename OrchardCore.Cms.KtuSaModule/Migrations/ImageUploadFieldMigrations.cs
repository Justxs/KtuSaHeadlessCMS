using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ImageUploadFieldMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync("ImageUploadPart", builder => builder
            .WithField(nameof(ImageUploadField), field => field
                .OfType(nameof(ImageUploadField))
            )
        );

        return 1;
    }
}