using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class GetStaticPageRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";

    [BindFrom("pageName")]
    [Description("Partial or full display text of the static page to retrieve")]
    public string PageName { get; set; } = null!;
}