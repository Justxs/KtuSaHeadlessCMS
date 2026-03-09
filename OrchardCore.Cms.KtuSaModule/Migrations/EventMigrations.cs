using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class EventMigrations(
    IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    private const int CurrentVersion = 13;

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

    public async Task<int> UpdateFrom6Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom7Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom8Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom9Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom10Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom11Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom12Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(EventPart), part => part
            .Attachable()
            .RemoveField("BodyFieldLt")
            .RemoveField("BodyFieldEn")
            .WithField(nameof(EventPart.CoverImage), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload cover image")
                .WithSettings(new MediaFieldSettings
                {
                    Required = true,
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
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
            .RemovePart("ContentLt")
            .RemovePart("ContentEn")
            .WithPart("ContentLt", nameof(FlowPart), part => part
                .WithDisplayName("Event Body (Lithuanian)")
                .WithPosition("3")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithPart("ContentEn", nameof(FlowPart), part => part
                .WithDisplayName("Event Body (English)")
                .WithPosition("4")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithDescription("Event content type"));
    }
}