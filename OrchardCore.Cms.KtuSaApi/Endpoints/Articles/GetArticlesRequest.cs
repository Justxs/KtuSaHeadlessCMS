using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;
    [Description("Optional. When provided, limits the number of articles returned")]
    public int? Limit { get; set; }
}