using FastEndpoints;
using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsRequest : PagedRequest
{
    [QueryParam]
    [Description("Response language. Allowed values: 'en' (default) and 'lt'.")]
    public Language Language { get; set; } = Language.EN;

    [QueryParam]
    [Description(
        "Optional case-insensitive text filter matched against the question and the plain-text answer.")]
    public string? Search { get; set; }

    [QueryParam]
    [Description(
        "Optional number of random FAQ items to return. When set, search and paging are ignored and the subset is returned as a single page.")]
    public int? Limit { get; set; }
}
