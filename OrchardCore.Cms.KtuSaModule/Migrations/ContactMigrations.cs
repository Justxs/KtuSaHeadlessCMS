using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
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
                .OfType(nameof(MemberPart.ImageUploadField))
                .WithDisplayName("Upload image"))
            .WithDescription("Member info: Name, Responsibilities, SaUnit, photo")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(PositionPart), part => part
            .Attachable()
            .WithDescription("Position info: Name and description")
        );

        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(AddressPart), part => part
            .Attachable()
            .WithDescription("Address part: location for an object like the office of KTU SA")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync("Member", type => type
            .Creatable()
            .Listable()
            .WithPart(nameof(MemberPart))
            .WithPart(nameof(PositionPart))
            .WithPart(nameof(ContactPart))
            .WithDescription("All info about member")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.MainContact.ToString(), type => type
            .Listable()
            .WithPart(nameof(ContactPart))
            .WithPart(nameof(AddressPart))
            .WithDescription("Main Contacts of KTU SA")
        );

        var mainContactItem = await contentManager.NewAsync(ContentTypeNames.MainContact.ToString());
        mainContactItem.DisplayText = "Pagrindiniai kontaktai / Main contacts";

        var contactPart = mainContactItem.As<ContactPart>();
        var addressPart = mainContactItem.As<AddressPart>();


        if (contactPart != null)
        {
            contactPart.PhoneNumber = "+37012345678";
            contactPart.Email = "info@example.com";
            addressPart.Address = "Kaunas, Lithuania";

            mainContactItem.Apply(nameof(ContactPart), contactPart);

            await contentManager.CreateAsync(mainContactItem);
        }
        await contentManager.CreateAsync(mainContactItem);

        await SchemaBuilder.CreateMapIndexTableAsync<MemberPartIndex>(table => table
            .Column<string>(nameof(MemberPartIndex.ContentItemId), column => column.WithLength(26))
            .Column<string>(nameof(MemberPartIndex.SaUnit), column => column.WithLength(10))
        );

        await SchemaBuilder.AlterTableAsync(nameof(MemberPartIndex), table => table
            .CreateIndex(
                $"IDX_{nameof(MemberPartIndex)}_{nameof(MemberPartIndex.SaUnit)}",
                nameof(MemberPartIndex.SaUnit))
        );

        return 2;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await SchemaBuilder.CreateMapIndexTableAsync<MemberPartIndex>(table => table
            .Column<string>(nameof(MemberPartIndex.ContentItemId), column => column.WithLength(26))
            .Column<string>(nameof(MemberPartIndex.SaUnit), column => column.WithLength(10))
        );

        await SchemaBuilder.AlterTableAsync(nameof(MemberPartIndex),table => table
            .CreateIndex(
            $"IDX_{nameof(MemberPartIndex)}_{nameof(MemberPartIndex.SaUnit)}",
            nameof(MemberPartIndex.SaUnit))
        );

        return 2;
    }
}