using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Models.Fields;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class EventMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(CardPart), part => part
            .Attachable()
            .WithField(nameof(CardPart.ImageUploadField), field => field
                .OfType(nameof(ImageUploadField))
                .WithDisplayName("Upload thumbnail image"))
            .WithDescription("Card part for displaying main info")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(EventPart), part => part
            .Attachable()
            .WithField(nameof(EventPart.SaUnit), field => field
                .OfType(nameof(SaUnitSelectField)))
            .WithField(nameof(EventPart.BodyFieldLt), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text LT")
                .WithPosition("7"))
            .WithField(nameof(EventPart.BodyFieldEn), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text EN")
                .WithPosition("8"))
            .WithDescription("Event part info")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Event.ToString(), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(CardPart), part => part
                .WithPosition("1"))
            .WithPart(nameof(EventPart), part => part
                .WithPosition("2"))
            .WithDescription("Event content type")
        );

        return 1;
    }
}