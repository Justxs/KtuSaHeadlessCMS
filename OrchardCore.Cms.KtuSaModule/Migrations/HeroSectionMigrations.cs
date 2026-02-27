using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class HeroSectionMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await contentDefinitionManager.AlterPartDefinitionAsync(nameof(HeroSectionPart), builder => builder
            .WithField(nameof(HeroSectionPart.DescriptionLt), field => field
                .OfType(nameof(TextField))
                .WithDisplayName("Description LT")
                .WithSettings(new TextFieldSettings
                {
                    Required = true,
                    Hint = "Hero content section in Lithuanian language"
                }))
            .WithField(nameof(HeroSectionPart.DescriptionEn), field => field
                .OfType(nameof(TextField))
                .WithDisplayName("Description EN")
                .WithSettings(new TextFieldSettings
                {
                    Required = true,
                    Hint = "Hero content section in English language"
                }))
            .WithField(nameof(HeroSectionPart.ImageUploadField), field => field
                .OfType(nameof(HeroSectionPart.ImageUploadField))
                .WithDisplayName("Upload hero image"))
            .WithDescription("Hero section content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(HeroSection, type => type
            .Listable()
            .WithPart(nameof(HeroSectionPart))
            .WithDescription("Hero sections for all pages")
        );

        await CreateHeroSectionAsync("Kas yra KTU SA?", "What is KTU SA?");
        await CreateHeroSectionAsync("Fakultetinės studentų atstovybės", "Faculty's student association");
        await CreateHeroSectionAsync("Veiklos ataskaitos", "Activity Reports");
        await CreateHeroSectionAsync("Dokumentai", "Documents");

        await CreateHeroSectionAsync("Bendrabučiai", "Dormitories");
        await CreateHeroSectionAsync("Stipendijos", "Scholarships");
        await CreateHeroSectionAsync("Straipsniai", "Articles");
        await CreateHeroSectionAsync("Artimiausi renginiai", "Upcoming events");

        await CreateHeroSectionAsync("Dažniausiai užduodami klausimai", "Frequently asked questions");
        await CreateHeroSectionAsync("Akademinė pagalba", "Academic help");
        await CreateHeroSectionAsync("Socialinė pagalba", "Social help");

        await CreateHeroSectionAsync("Seniūnai", "Elders");
        await CreateHeroSectionAsync("Studentų atstovai KTU organuose", "Student representatives in KTU bodies");
        await CreateHeroSectionAsync("Studentų atstovai fakultetų organuose",
            "Student representatives in faculties bodies");

        await CreateHeroSectionAsync("Kontaktai", "Contacts");

        return 1;
    }

    public async Task CreateHeroSectionAsync(string titleLt, string titleEn)
    {
        var contactPageHero = await contentManager.NewAsync(HeroSection);
        contactPageHero.DisplayText = $"{titleLt} / {titleEn}";

        var heroSectionPart = contactPageHero.As<HeroSectionPart>();

        heroSectionPart.TitleLt = titleLt;
        heroSectionPart.TitleEn = titleEn;

        contactPageHero.Apply(nameof(HeroSectionPart), heroSectionPart);
        await contentManager.CreateAsync(contactPageHero);
    }
}