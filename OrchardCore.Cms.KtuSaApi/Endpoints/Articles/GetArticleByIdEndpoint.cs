using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticleByIdEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetArticleByIdRequest, ArticleContentResponse>
{
    public override void Configure()
    {
        Get("api/articles/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Articles")
            .WithSummary("Get an article by ID")
            .WithDescription(
                "Returns a published article by content item ID, including structured content blocks, reading time, and heading list. " +
                "Use query parameter language=en (default) or language=lt.")
            .Produces<ArticleContentResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetArticleByIdRequest req, CancellationToken ct)
    {
        var article = await repository.GetByIdAsync(req.Id);

        if (article is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var language = req.Language;

        await Send.OkAsync(article.ToContentResponse(language, mediaFileStore), ct);
    }
}
