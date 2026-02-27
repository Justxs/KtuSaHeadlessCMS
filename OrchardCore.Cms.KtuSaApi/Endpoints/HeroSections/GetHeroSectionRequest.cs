namespace OrchardCore.Cms.KtuSaApi.Endpoints.HeroSections;

public class GetHeroSectionRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;

    [Description("Partial or full title of the hero section in the requested language")]
    public string SectionName { get; set; } = null!;
}