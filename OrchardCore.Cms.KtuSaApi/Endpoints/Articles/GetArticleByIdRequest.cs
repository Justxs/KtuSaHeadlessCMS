using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticleByIdRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";

    [BindFrom("id")]
    [Description("Content item ID of the article")]
    public string Id { get; set; } = null!;
}