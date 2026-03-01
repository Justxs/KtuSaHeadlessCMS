using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class EventMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 6;

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
        await contentDefinitionManager.DeleteTypeDefinitionAsync(Event);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(EventPart));
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(EventPart), part => part
            .Attachable()
            .WithField(nameof(EventPart.CoverImage), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload cover image")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithField(nameof(EventPart.BodyFieldLt), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text LT")
                .WithPosition("7"))
            .WithField(nameof(EventPart.BodyFieldEn), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text EN")
                .WithPosition("8"))
            .WithField(nameof(EventPart.OrganisersField), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select event organisers")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = true,
                    DisplayedContentTypes = [SaUnit],
                    Required = true
                })
                .WithDescription("Event part info")));

        await contentDefinitionManager.AlterTypeDefinitionAsync(Event, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(EventPart), part => part
                .WithPosition("2"))
            .WithDescription("Event content type"));
    }
}
