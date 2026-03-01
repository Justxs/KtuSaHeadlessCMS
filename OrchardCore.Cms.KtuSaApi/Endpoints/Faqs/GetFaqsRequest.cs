using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";

    [QueryParam]
    [Description("Optional. When provided, returns a random subset of that many items")]
    public int? Limit { get; set; }
}
