using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SponsorMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(SponsorPart), part =>
            part.Attachable()
                .WithField(nameof(SponsorPart.ImageUploadField), field => field
                    .OfType(nameof(ImageUploadField))
                    .WithDisplayName("Upload company Logo"))
                .WithDescription("Sponsors content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(Sponsor, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(SponsorPart))
            .WithDescription("Sponsors content type")
        );

        return 1;
    }
}