using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class UserMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(UserProfilePart), part =>
            part.Attachable()
                .WithField(nameof(UserProfilePart.SaUnit), field => field
                    .OfType(nameof(SaUnitSelectField)))
                .WithDescription("User SA unit")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(nameof(ContentTypeNames.UserProfile), type => type
            .WithPart(nameof(UserProfilePart))
            .Stereotype("CustomUserSettings")
        );

        return 1;
    }
}