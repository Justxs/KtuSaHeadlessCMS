using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;
using SaUnit = OrchardCore.Cms.KtuSaModule.Models.Enums.SaUnit;

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
            .WithField(nameof(MemberPart.SaUnit), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select SA unit")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = [ContentTypeConstants.SaUnit],
                }))
            .WithField(nameof(MemberPart.Position), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select position")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = [Position],
                }))
            .WithDescription("Member info: Name, Responsibilities, Sa Unit, photo")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(AddressPart), part => part
            .Attachable()
            .WithDescription("Address part: location for an object like the office of KTU SA")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(Contact, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(MemberPart))
            .WithPart(nameof(ContactPart))
            .WithDescription("All info about member")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(MainContact, type => type
            .Listable()
            .WithPart(nameof(ContactPart))
            .WithPart(nameof(AddressPart))
            .WithDescription("Main Contacts of KTU SA")
        );

        await CreateMainContactAsync(SaUnit.CSA);

        return 1;
    }

    private async Task CreateMainContactAsync(SaUnit saUnit)
    {
        var mainContactItem = await contentManager.NewAsync(MainContact);
        mainContactItem.DisplayText = saUnit.ToString();

        var contactPart = mainContactItem.As<ContactPart>();
        var addressPart = mainContactItem.As<AddressPart>();

        contactPart.PhoneNumber = "+37012345678";
        contactPart.Email = "info@example.com";
        addressPart.Address = "Kaunas, Lithuania";
        addressPart.SaUnit = "CSA";

        mainContactItem.Apply(nameof(ContactPart), contactPart);
        mainContactItem.Apply(nameof(AddressPart), addressPart);

        await contentManager.CreateAsync(mainContactItem);
    }
}