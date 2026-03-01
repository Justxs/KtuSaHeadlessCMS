using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";
    [Description("Optional. When provided, limits the number of articles returned")]
    public int? Limit { get; set; }
}