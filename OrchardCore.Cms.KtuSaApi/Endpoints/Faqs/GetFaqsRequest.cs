using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;

    [QueryParam]
    [Description("Optional. When provided, returns a random subset of that many items")]
    public int? Limit { get; set; }
}
