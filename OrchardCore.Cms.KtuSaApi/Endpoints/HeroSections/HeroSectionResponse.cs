namespace OrchardCore.Cms.KtuSaApi.Endpoints.HeroSections;

public class HeroSectionResponse
{
    [Description("Hero section title in the requested language")]
    public string Title { get; set; } = null!;

    [Description("Hero section description text in the requested language")]
    public string Description { get; set; } = null!;

    [Description("File ID of the hero section background image")]
    public string ImgSrc { get; set; } = null!;
}