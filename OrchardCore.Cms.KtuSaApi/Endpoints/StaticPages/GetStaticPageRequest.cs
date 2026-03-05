using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class GetStaticPageRequest
{
    [QueryParam]
    [Description("Response language. Allowed values: 'en' (default) and 'lt'.")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("pageName")]
    [Description("Case-insensitive full or partial title match used to locate the static page.")]
    public string PageName { get; set; } = null!;
}
