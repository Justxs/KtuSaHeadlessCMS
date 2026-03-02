using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticleByIdRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("id")]
    [Description("Content item ID of the article")]
    public string Id { get; set; } = null!;
}