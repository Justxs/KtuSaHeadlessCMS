namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticleByIdRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;

    [Description("Content item ID of the article")]
    public string Id { get; set; } = null!;
}
