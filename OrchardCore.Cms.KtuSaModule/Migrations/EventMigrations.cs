using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class EventMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(EventPart), part => part
            .Attachable()
            .WithField(nameof(EventPart.BodyFieldLt), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text LT")
                .WithPosition("7"))
            .WithField(nameof(EventPart.BodyFieldEn), field => field
                .OfType(nameof(QuillField))
                .WithDisplayName("Event text EN")
                .WithPosition("8"))
            .WithField("OrganisersField", field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select event organisers")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = true,
                    DisplayedContentTypes = [nameof(ContentTypeNames.SaUnit)],
                })
            .WithDescription("Event part info"))
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(nameof(ContentTypeNames.Event), type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(EventPart), part => part
                .WithPosition("2"))
            .WithDescription("Event content type")
        );

        return 1;
    }
}