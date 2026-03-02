using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class GetStaticPageRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("pageName")]
    [Description("Partial or full display text of the static page to retrieve")]
    public string PageName { get; set; } = null!;
}