using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetArticlesRequest, List<ArticlePreviewResponse>>
{
    public override void Configure()
    {
        Get("api/articles");
        AllowAnonymous();
        Description(b => b
            .WithTags("Articles")
            .WithSummary("List article previews")
            .WithDescription(
                "Returns published article previews ordered by creation date descending. " +
                "Use query parameter language=en (default) or language=lt. Optional query parameter limit caps the number of returned items.")
            .Produces<List<ArticlePreviewResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetArticlesRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Article);
        var language = req.Language;

        IEnumerable<ArticlePreviewResponse> response = query
            .Select(item => item.ToPreviewResponse(language, mediaFileStore))
            .OrderByDescending(item => item.CreatedDate);

        if (req.Limit is not null) response = response.Take(req.Limit.Value);

        await Send.OkAsync([.. response], ct);
    }
}