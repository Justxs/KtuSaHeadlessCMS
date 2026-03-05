using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class SaUnitMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager)
    : DataMigration
{
    private const int CurrentVersion = 7;

    public async Task<int> CreateAsync()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();

        foreach (var saUnit in Enum.GetValues<SaUnit>())
        {
            await CreateSaUnitsAsync(saUnit);
        }

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync(ContentTypeConstants.SaUnit);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(SaUnitPart));
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await AlterPartDefinitionAsync();
        await AlterTypeDefinitionAsync();
        return CurrentVersion;
    }

    private Task AlterPartDefinitionAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(SaUnitPart), part => part
            .Attachable()
            .WithField(nameof(SaUnitPart.UnitPhoto), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload FSA photo")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithDescription("Sa unit content part"));
    }

    private Task AlterTypeDefinitionAsync()
    {
        return contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeConstants.SaUnit, type => type
            .Listable()
            .WithPart(nameof(SaUnitPart), part => part.WithPosition("1"))
            .RemovePart(nameof(ContactPart))
            .RemovePart("ContentLt")
            .RemovePart("ContentEn")
            .WithPart("ContentLt", nameof(FlowPart), part => part
                .WithDisplayName("Body (Lithuanian)")
                .WithPosition("2")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes = [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithPart("ContentEn", nameof(FlowPart), part => part
                .WithDisplayName("Body (English)")
                .WithPosition("3")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes = [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithDescription("Sa unit content type"));
    }

    private async Task CreateSaUnitsAsync(SaUnit saUnit)
    {
        var saUnitItem = await contentManager.NewAsync(ContentTypeConstants.SaUnit);
        saUnitItem.DisplayText = saUnit.ToString().Replace("_", " ");

        var saUnitPart = saUnitItem.As<SaUnitPart>();

        saUnitPart.UnitName = saUnit.ToString();

        saUnitItem.Apply(nameof(SaUnitPart), saUnitPart);

        await contentManager.CreateAsync(saUnitItem);
    }
}
