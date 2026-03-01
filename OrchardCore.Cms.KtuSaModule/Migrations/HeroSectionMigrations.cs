using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class HeroSectionMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
    : DataMigration
{
    private const int CurrentVersion = 2;

    public async Task<int> CreateAsync()
    {
        await ApplySchemaAsync();

        await CreateHeroSectionAsync("Kas yra KTU SA?", "What is KTU SA?");
        await CreateHeroSectionAsync("Fakultetin\u0117s student\u0173 atstovyb\u0117s", "Faculty's student association");
        await CreateHeroSectionAsync("Veiklos ataskaitos", "Activity Reports");
        await CreateHeroSectionAsync("Dokumentai", "Documents");

        await CreateHeroSectionAsync("Bendrabu\u010diai", "Dormitories");
        await CreateHeroSectionAsync("Stipendijos", "Scholarships");
        await CreateHeroSectionAsync("Straipsniai", "Articles");
        await CreateHeroSectionAsync("Artimiausi renginiai", "Upcoming events");

        await CreateHeroSectionAsync("Da\u017eniausiai u\u017eduodami klausimai", "Frequently asked questions");
        await CreateHeroSectionAsync("Akademin\u0117 pagalba", "Academic help");
        await CreateHeroSectionAsync("Socialin\u0117 pagalba", "Social help");

        await CreateHeroSectionAsync("Seni\u016bnai", "Elders");
        await CreateHeroSectionAsync("Student\u0173 atstovai KTU organuose", "Student representatives in KTU bodies");
        await CreateHeroSectionAsync("Student\u0173 atstovai fakultet\u0173 organuose",
            "Student representatives in faculties bodies");

        await CreateHeroSectionAsync("Kontaktai", "Contacts");

        return CurrentVersion;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await ApplySchemaAsync();
        return CurrentVersion;
    }

    private async Task ApplySchemaAsync()
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
                .OfType(nameof(MediaField))
                .WithDisplayName("Upload hero image")
                .WithSettings(new MediaFieldSettings
                {
                    Multiple = false,
                    AllowMediaText = false,
                    AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"]
                }))
            .WithDescription("Hero section content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync("HeroSection", type => type
            .Listable()
            .WithPart(nameof(HeroSectionPart))
            .WithDescription("Hero sections for all pages")
        );
    }

    public async Task CreateHeroSectionAsync(string titleLt, string titleEn)
    {
        var contactPageHero = await contentManager.NewAsync("HeroSection");
        contactPageHero.DisplayText = $"{titleLt} / {titleEn}";

        var heroSectionPart = contactPageHero.As<HeroSectionPart>();

        heroSectionPart.TitleLt = titleLt;
        heroSectionPart.TitleEn = titleEn;

        contactPageHero.Apply(nameof(HeroSectionPart), heroSectionPart);
        await contentManager.CreateAsync(contactPageHero);
    }
}
