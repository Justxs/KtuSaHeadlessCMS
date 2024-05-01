using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class StaticPageMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(StaticPagePart), part =>
            part.Attachable()
                .WithField(nameof(StaticPagePart.BodyLt), field => field
                    .OfType(nameof(QuillField))
                    .WithDisplayName("Page body LT"))
                .WithField(nameof(StaticPagePart.BodyEn), field => field
                    .OfType(nameof(QuillField))
                    .WithDisplayName("Page body EN"))
                .WithDescription("Static page content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(StaticPage, type => type
            .Listable()
            .WithPart(nameof(StaticPagePart))
            .WithDescription("Static page content type")
        );

        await CreateStaticPagesAsync("Kas yra KTU SA?", "What is KTU SA?");
        await CreateStaticPagesAsync("Stipendijos", "Scholarships");
        await CreateStaticPagesAsync("Seniūnai", "Elders");
        await CreateStaticPagesAsync("Studentų atstovai fakultetų organuose", "Student representatives in faculties bodies");
        await CreateStaticPagesAsync("Studentų atstovai KTU organuose", "Student Representatives in KTU Bodies");
        await CreateStaticPagesAsync("Socialinė pagalba", "Social Help");
        await CreateStaticPagesAsync("Akademinė pagalba", "Academic Help");

        return 1;
    }

    public async Task CreateStaticPagesAsync(string titleLt, string titleEn)
    {
        var contactPageHero = await contentManager.NewAsync(StaticPage);
        contactPageHero.DisplayText = $"{titleLt} / {titleEn}";

        var staticPagePart = contactPageHero.As<StaticPagePart>();

        staticPagePart.TitleLt = titleLt;
        staticPagePart.TitleEn = titleEn;

        contactPageHero.Apply(nameof(staticPagePart), staticPagePart);
        await contentManager.CreateAsync(contactPageHero);
    }
}