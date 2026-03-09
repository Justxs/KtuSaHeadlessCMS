using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using YesSql;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class StaticPageMigrations(
    IContentDefinitionManager contentDefinitionManager,
    IContentManager contentManager,
    ISession session)
    : DataMigration
{
    private const int CurrentVersion = 15;

    public async Task<int> CreateAsync()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();

        await CreateStaticPagesAsync("Kas yra KTU SA?", "What is KTU SA?");
        await CreateStaticPagesAsync("Stipendijos", "Scholarships");
        await CreateStaticPagesAsync("Seni\u016bnai", "Elders");
        await CreateStaticPagesAsync("Student\u0173 atstovai fakultet\u0173 organuose",
            "Student representatives in faculties bodies");
        await CreateStaticPagesAsync("Student\u0173 atstovai KTU organuose", "Student Representatives in KTU Bodies");
        await CreateStaticPagesAsync("Socialin\u0117 pagalba", "Social Help");
        await CreateStaticPagesAsync("Akademin\u0117 pagalba", "Academic Help");

        return 1;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();

        var heroSections = await session.Query<ContentItem, ContentItemIndex>(x =>
            x.ContentType == "HeroSection" && x.Published).ListAsync();

        var staticPages = await session.Query<ContentItem, ContentItemIndex>(x =>
            x.ContentType == StaticPage && x.Published).ListAsync();

        var staticPagesByTitle = staticPages
            .ToDictionary(p => p.DisplayText, p => p);

        foreach (var hero in heroSections)
        {
            var heroPart = hero.As<HeroSectionPart>();
            if (heroPart is null) continue;

            var heroImagePath = heroPart.ImageUploadField?.Paths?.FirstOrDefault();

            var displayText = hero.DisplayText;

            if (staticPagesByTitle.TryGetValue(displayText, out var existingPage))
            {
                var pagePart = existingPage.As<StaticPagePart>();
                pagePart.DescriptionLt = heroPart.DescriptionLt;
                pagePart.DescriptionEn = heroPart.DescriptionEn;
                pagePart.HeroImage = CreateMediaField(heroImagePath);

                existingPage.Apply(nameof(StaticPagePart), pagePart);
                await contentManager.UpdateAsync(existingPage);
            }
            else
            {
                var newPage = await contentManager.NewAsync(StaticPage);
                newPage.DisplayText = displayText;

                var pagePart = newPage.As<StaticPagePart>();
                pagePart.TitleLt = heroPart.TitleLt;
                pagePart.TitleEn = heroPart.TitleEn;
                pagePart.DescriptionLt = heroPart.DescriptionLt;
                pagePart.DescriptionEn = heroPart.DescriptionEn;
                pagePart.HeroImage = CreateMediaField(heroImagePath);

                newPage.Apply(nameof(StaticPagePart), pagePart);
                await contentManager.CreateAsync(newPage);
            }

            await contentManager.RemoveAsync(hero);
        }

        await contentDefinitionManager.DeleteTypeDefinitionAsync("HeroSection");
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(HeroSectionPart));

        return 2;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom4Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom5Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom6Async()
    {
        await contentDefinitionManager.DeleteTypeDefinitionAsync(StaticPage);
        await contentDefinitionManager.DeletePartDefinitionAsync(nameof(StaticPagePart));
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom7Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom8Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom9Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom10Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom11Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom12Async()
    {
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return 13;
    }

    public async Task<int> UpdateFrom13Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        await AlterTypeDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    public async Task<int> UpdateFrom14Async()
    {
        await AlterPartDefinitionForCurrentSchemaAsync();
        return CurrentVersion;
    }

    private Task AlterPartDefinitionForCurrentSchemaAsync()
    {
        return contentDefinitionManager.AlterPartDefinitionAsync(nameof(StaticPagePart), part =>
            part.RemoveField("BodyLt")
                .RemoveField("BodyEn")
                .WithField(nameof(StaticPagePart.DescriptionLt), field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Description LT")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = false,
                        Hint = "Hero content section in Lithuanian language"
                    }))
                .WithField(nameof(StaticPagePart.DescriptionEn), field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Description EN")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = false,
                        Hint = "Hero content section in English language"
                    }))
                .WithField(nameof(StaticPagePart.HeroImage), field => field
                    .OfType(nameof(MediaField))
                    .WithDisplayName("Upload hero image")
                    .WithPosition("2")
                    .WithSettings(new MediaFieldSettings
                    {
                        Required = true,
                        Multiple = false,
                        AllowMediaText = false,
                        AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                    }))
        );
    }

    private Task AlterTypeDefinitionForCurrentSchemaAsync()
    {
        return contentDefinitionManager.AlterTypeDefinitionAsync(StaticPage, type => type
            .Listable()
            .WithPart(nameof(StaticPagePart))
            .RemovePart("ContentLt")
            .RemovePart("ContentEn")
            .WithPart("ContentLt", nameof(FlowPart), part => part
                .WithDisplayName("Body (Lithuanian)")
                .WithPosition("3")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithPart("ContentEn", nameof(FlowPart), part => part
                .WithDisplayName("Body (English)")
                .WithPosition("4")
                .WithSettings(new FlowPartSettings
                {
                    ContainedContentTypes =
                        [ParagraphWidget, ImageWidget, VideoWidget, PdfDocumentWidget, ImageCarouselWidget]
                }))
            .WithDescription("Static page content type"));
    }

    private static MediaField CreateMediaField(string? path)
    {
        var mediaField = new MediaField();

        if (!string.IsNullOrWhiteSpace(path)) mediaField.Paths = [path];

        return mediaField;
    }

    public async Task CreateStaticPagesAsync(string titleLt, string titleEn)
    {
        var contactPageHero = await contentManager.NewAsync(StaticPage);
        contactPageHero.DisplayText = $"{titleLt} / {titleEn}";

        var staticPagePart = contactPageHero.As<StaticPagePart>();

        staticPagePart.TitleLt = titleLt;
        staticPagePart.TitleEn = titleEn;

        contactPageHero.Apply(nameof(StaticPagePart), staticPagePart);
        await contentManager.CreateAsync(contactPageHero);
    }
}