using FastEndpoints;
using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesRequest : PagedRequest
{
    [QueryParam]
    [Description("Response language. Allowed values: 'en' (default) and 'lt'.")]
    public Language Language { get; set; } = Language.EN;

    [QueryParam]
    [Description(
        "Optional maximum number of articles to consider. Applied before paging, so it caps the total, not the page.")]
    public int? Limit { get; set; }
}
