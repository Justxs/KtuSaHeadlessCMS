using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class UserMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(UserProfilePart), part =>
            part.Attachable()
                .WithField(nameof(UserProfilePart.SaUnit), field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Select SA unit")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Multiple = false,
                        Required = true,
                        DisplayedContentTypes = [SaUnit]
                    }))
                .WithDescription("User SA unit")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(UserProfile, type => type
            .WithPart(nameof(UserProfilePart))
            .Stereotype("CustomUserSettings")
        );

        return 1;
    }
}