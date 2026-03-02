using OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class WidgetMigrations(IContentDefinitionManager contentDefinitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await DefineParagraphWidgetAsync();
        await DefineImageWidgetAsync();
        await DefineVideoWidgetAsync();
        await DefinePdfDocumentWidgetAsync();
        await DefineImageCarouselWidgetAsync();

        return 6;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync("RichTextWidget");
        await DefineParagraphWidgetAsync();
        await DefineImageWidgetAsync();
        await DefineVideoWidgetAsync();
        return 5;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await DefineParagraphWidgetAsync();
        return 5;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await DefineParagraphWidgetAsync();
        return 5;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync(HeadingWidget);
        await contentDefinitionManager.DeletePartDefinitionAsync("HeadingWidgetPart");
        await DefineParagraphWidgetAsync();
        return 5;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await DefinePdfDocumentWidgetAsync();
        await DefineImageCarouselWidgetAsync();
        return 6;
    }

    private async Task DefineParagraphWidgetAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ParagraphWidgetPart), part => part
            .RemoveField("Text")
            .WithField(nameof(ParagraphWidgetPart.Body), field => field
                .OfType(nameof(HtmlField))
                .WithDisplayName("Content")
                .WithEditor("Trumbowyg")
                .WithSettings(new HtmlFieldSettings
                {
                    Hint = "Enter formatted text (bold, italic, links)"
                })
                .WithSettings(new HtmlFieldTrumbowygEditorSettings
                {
                    Options = """
                              {
                                  "btns": [
                                      ["undo", "redo"],
                                      ["formatting"],
                                      ["strong", "em"],
                                      ["link"],
                                      ["unorderedList", "orderedList"]
                                  ]
                              }
                              """
                })));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ParagraphWidget, type => type
            .Stereotype("Widget")
            .DisplayedAs("Paragraph")
            .WithDescription("A block of text")
            .WithPart(nameof(ParagraphWidgetPart)));
    }

    private async Task DefineImageWidgetAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ImageWidgetPart), part => part
            .WithField(nameof(ImageWidgetPart.Image), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Image")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                })));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ImageWidget, type => type
            .Stereotype("Widget")
            .DisplayedAs("Image")
            .WithDescription("A standalone image block")
            .WithPart(nameof(ImageWidgetPart)));
    }

    private async Task DefineVideoWidgetAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(VideoWidgetPart), part => part
            .WithField(nameof(VideoWidgetPart.Url), field => field
                .OfType(nameof(TextField))
                .WithDisplayName("Video URL")
                .WithSettings(new TextFieldSettings
                {
                    Required = true,
                    Hint = "YouTube or other video embed URL"
                })));

        await contentDefinitionManager.AlterTypeDefinitionAsync(VideoWidget, type => type
            .Stereotype("Widget")
            .DisplayedAs("Video")
            .WithDescription("An embedded video block")
            .WithPart(nameof(VideoWidgetPart)));
    }

    private async Task DefinePdfDocumentWidgetAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(PdfDocumentWidgetPart), part => part
            .WithField(nameof(PdfDocumentWidgetPart.Document), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("PDF Document")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".pdf"]
                })));

        await contentDefinitionManager.AlterTypeDefinitionAsync(PdfDocumentWidget, type => type
            .Stereotype("Widget")
            .DisplayedAs("PDF Document")
            .WithDescription("An embedded PDF document block")
            .WithPart(nameof(PdfDocumentWidgetPart)));
    }

    private async Task DefineImageCarouselWidgetAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(ImageCarouselWidgetPart), part => part
            .WithField(nameof(ImageCarouselWidgetPart.Images), field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Carousel Images")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = true,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                })));

        await contentDefinitionManager.AlterTypeDefinitionAsync(ImageCarouselWidget, type => type
            .Stereotype("Widget")
            .DisplayedAs("Image Carousel")
            .WithDescription("A carousel of multiple images")
            .WithPart(nameof(ImageCarouselWidgetPart)));
    }
}
