using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using YesSql;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;
using SaUnit = OrchardCore.Cms.KtuSaModule.Models.Enums.SaUnit;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class ContactMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    ISession session) : DataMigration
{
    private const int CurrentVersion = 9;

    public async Task<int> CreateAsync()
    {
        await UpdateSchemaToCurrentAsync();

        await CreateMainContactAsync(SaUnit.CSA);

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await MigrateEmailToMemberPartAsync();

        await contentDefinitionManager.AlterTypeDefinitionAsync(Contact, type => type
            .RemovePart(nameof(ContactPart)));

        await UpdateSchemaToCurrentAsync();

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom7Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync(Contact);
        await contentDefinitionManager.DeleteTypeDefinitionAsync(MainContact);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(MemberPart));
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(ContactPart));
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(AddressPart));
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom8Async()
    {
        await UpdateSchemaToCurrentAsync();
        return CurrentVersion;
    }

    private async Task UpdateSchemaToCurrentAsync()
    {
        await AlterContactPartDefinitionAsync();
        await AlterMemberPartDefinitionAsync();
        await AlterAddressPartDefinitionAsync();
        await ConfigureContentTypesAsync();
    }

    private Task AlterContactPartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(ContactPart), part => part
            .Attachable()
            .WithField(nameof(ContactPart.Photo), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload Contact photo")
                .WithSettings(new MediaFieldSettings
                {
                    Required = true,
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithDescription("Contact part for showing Phone number, email and address"));
    }

    private Task AlterMemberPartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(MemberPart), part => part
            .Attachable()
            .WithField(nameof(MemberPart.MemberPhoto), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload Member photo")
                .WithSettings(new MediaFieldSettings
                {
                    Required = true,
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithField(nameof(MemberPart.SaUnit), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select SA unit")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = [ContentTypeConstants.SaUnit]
                }))
            .WithField(nameof(MemberPart.Position), field => field
                .OfType(nameof(ContentPickerField))
                .WithDisplayName("Select position")
                .WithSettings(new ContentPickerFieldSettings
                {
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = [Position]
                }))
            .WithDescription("Member info: Name, Responsibilities, Sa Unit, photo"));
    }

    private Task AlterAddressPartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(AddressPart), part => part
            .Attachable()
            .WithDescription("Address part: location for an object like the office of KTU SA"));
    }

    private async Task ConfigureContentTypesAsync()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(Contact, type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(MemberPart))
            .WithDescription("All info about member"));

        await contentDefinitionManager.AlterTypeDefinitionAsync(MainContact, type => type
            .Listable()
            .WithPart(nameof(ContactPart))
            .WithPart(nameof(AddressPart))
            .WithDescription("Main Contacts of KTU SA"));
    }

    private async Task MigrateEmailToMemberPartAsync()
    {
        var contentItems = await session
            .Query<ContentItem>()
            .With<ContentItemIndex>(x => x.ContentType == Contact)
            .ListAsync();

        foreach (var contentItem in contentItems)
        {
            var contactPart = contentItem.As<ContactPart>();
            var memberPart = contentItem.As<MemberPart>();

            if (contactPart == null || memberPart == null) continue;

            memberPart.Email = contactPart.Email;

            contentItem.Remove(nameof(ContactPart));

            contentItem.Apply(nameof(MemberPart), memberPart);
            await contentManager.UpdateAsync(contentItem);
        }
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