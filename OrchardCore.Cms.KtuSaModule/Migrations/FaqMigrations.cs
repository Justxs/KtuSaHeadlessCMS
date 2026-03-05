using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Lists.Models;
using YesSql;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class FaqMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    ISession session) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(FaqPart), part => part
            .Attachable()
            .WithDescription("Frequently asked questions content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(Faq, type => type
            .Draftable()
            .Creatable()
            .Listable()
            .WithPart(nameof(FaqPart))
            .WithDescription("Frequently asked questions content type")
        );

        return 1;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(Faq, type => type
            .Draftable()
            .Creatable(false)
            .Listable(false)
            .WithPart(nameof(FaqPart))
            .WithDescription("Frequently asked question item")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(FaqPage, type => type
            .Listable()
            .Draftable()
            .WithPart("ListPart", part => part
                .MergeSettings<ListPartSettings>(s =>
                {
                    s.ContainedContentTypes = [Faq];
                    s.EnableOrdering = true;
                })
            )
            .WithDescription("FAQ page - container for all FAQ items")
        );

        var faqPage = await contentManager.NewAsync(FaqPage);
        faqPage.DisplayText = "FAQ";
        await contentManager.CreateAsync(faqPage, VersionOptions.Published);

        var existingFaqs = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == Faq && index.Published)
            .ListAsync();

        var order = 0;
        foreach (var faq in existingFaqs)
        {
            faq.Alter<ContainedPart>(part =>
            {
                part.ListContentItemId = faqPage.ContentItemId;
                part.Order = order++;
            });

            await contentManager.UpdateAsync(faq);
        }

        return 2;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await contentDefinitionManager.AlterTypeDefinitionAsync(Faq, type => type
            .Draftable()
            .Creatable(false)
            .Listable(false)
            .WithPart(nameof(FaqPart), part => part.WithPosition("1"))
            .WithPart("AnswerLt", nameof(FlowPart), part => part
                .WithDisplayName("Answer (Lithuanian)")
                .WithPosition("2")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithPart("AnswerEn", nameof(FlowPart), part => part
                .WithDisplayName("Answer (English)")
                .WithPosition("3")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithDescription("Frequently asked question item")
        );

        return 3;
    }
}