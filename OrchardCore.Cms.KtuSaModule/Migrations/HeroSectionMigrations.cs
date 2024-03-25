using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Cms.KtuSaModule.Migrations;

public class HeroSectionMigrations(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) : DataMigration
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
                        Hint = "Hero content section in Lithuanian language",
                    }))
                .WithField(nameof(HeroSectionPart.DescriptionEn), field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Description EN")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                        Hint = "Hero content section in English language",
                    }))
                .WithField(nameof(HeroSectionPart.ImageUploadField), field => field
                    .OfType(nameof(HeroSectionPart.ImageUploadField))
                    .WithDisplayName("Upload hero image"))
                .WithDescription("Hero section content part")
        );

        await contentDefinitionManager.AlterTypeDefinitionAsync(ContentTypeNames.HeroSection.ToString(), type => type
            .Listable()
            .WithPart(nameof(HeroSectionPart))
            .WithDescription("Hero sections for all pages")
        );

        await CreateHeroSectionAsync("Kontaktai", "Contacts");
        await CreateHeroSectionAsync("Bendrabučiai", "Dormitories");
        await CreateHeroSectionAsync("Artimiausi renginiai", "Upcoming events");
        await CreateHeroSectionAsync("Straipsniai", "Articles");
        await CreateHeroSectionAsync("Dokumentai", "Documents");
        await CreateHeroSectionAsync("Kas yra KTU SA?", "What is KTU SA?");
        await CreateHeroSectionAsync("Veiklos ataskaitos", "Activity Reports");
        await CreateHeroSectionAsync("Stipendijos", "Scholarships");
        await CreateHeroSectionAsync("Seniūnai", "Elders");
        await CreateHeroSectionAsync("Dažniausiai užduodami klausimai", "Frequently asked questions");

        return 1;
    }

    public async Task CreateHeroSectionAsync(string titleLt, string titleEn)
    {
        var contactPageHero = await contentManager.NewAsync(ContentTypeNames.HeroSection.ToString());
        contactPageHero.DisplayText = $"{titleLt} / {titleEn}";

        var heroSectionPart = contactPageHero.As<HeroSectionPart>();

        heroSectionPart.TitleLt = titleLt;
        heroSectionPart.TitleEn = titleEn;

        contactPageHero.Apply(nameof(HeroSectionPart), heroSectionPart);
        await contentManager.CreateAsync(contactPageHero);
    }
}