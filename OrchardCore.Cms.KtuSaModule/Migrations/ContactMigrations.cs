using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ContactMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ContactPart), part => part
            .Attachable()
            .WithDescription("Contact part for showing Phone number, email and address")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(MemberPart), part => part
            .Attachable()
            .WithField(nameof(MemberPart.ImageUploadField), field => field
                .OfType(nameof(ImageUploadField))
                .WithDisplayName("Upload Member photo"))
            .WithField(nameof(MemberPart.SaUnitSelectField), field => field
                .OfType(nameof(SaUnitSelectField)))
            .WithField(nameof(MemberPart.Position), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select position")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = [nameof(ContentTypeNames.Position)],
                }))
            .WithDescription("Member info: Name, Responsibilities, Sa Unit, photo")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(AddressPart), part => part
            .Attachable()
            .WithField(nameof(AddressPart.SaUnitSelectField), field => field
                .OfType(nameof(SaUnitSelectField)))
            .WithDescription("Address part: location for an object like the office of KTU SA")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.Contact.ToString(), type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(MemberPart))
            .WithPart(nameof(ContactPart))
            .WithDescription("All info about member")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.MainContact.ToString(), type => type
            .Listable()
            .WithPart(nameof(ContactPart))
            .WithPart(nameof(AddressPart))
            .WithDescription("Main Contacts of KTU SA")
        );

        await SchemaBuilder.CreateMapIndexTableAsync<MemberPartIndex>(table => table
            .Column<string>(nameof(MemberPartIndex.ContentItemId), column => column.WithLength(26))
            .Column<string>(nameof(MemberPartIndex.SaUnit), column => column.WithLength(10))
        );

        await SchemaBuilder.AlterTableAsync(nameof(MemberPartIndex), table => table
            .CreateIndex(
                $"IDX_{nameof(MemberPartIndex)}_{nameof(MemberPartIndex.SaUnit)}",
                nameof(MemberPartIndex.SaUnit))
        );

        foreach (var saUnit in (SaUnit[])Enum.GetValues(typeof(SaUnit)))
        {
            await CreateMainContactAsync(saUnit);
        }

        return 1;
    }

    private async Task CreateMainContactAsync(SaUnit saUnit)
    {
        var mainContactItem = await contentManager.NewAsync(ContentTypeNames.MainContact.ToString());
        mainContactItem.DisplayText = saUnit.ToString();

        var contactPart = mainContactItem.As<ContactPart>();
        var addressPart = mainContactItem.As<AddressPart>();

        contactPart.PhoneNumber = "+37012345678";
        contactPart.Email = "info@example.com";
        addressPart.Address = "Kaunas, Lithuania";

        mainContactItem.Apply(nameof(ContactPart), contactPart);
        mainContactItem.Apply(nameof(AddressPart), addressPart);

        await contentManager.CreateAsync(mainContactItem);
    }
}