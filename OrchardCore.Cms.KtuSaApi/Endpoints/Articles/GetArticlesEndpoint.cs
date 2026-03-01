using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
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
            .WithSummary("Get all articles")
            .WithDescription(
                "Returns a list of article previews ordered by creation date descending. " +
                "Pass language=lt or language=en. Query parameter 'limit' optionally caps the number of results.")
            .Produces<List<ArticlePreviewResponse>>(200));
    }

    public override async Task HandleAsync(GetArticlesRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Article);
        var isLithuanian = req.Language.IsLtLanguage();

        if (req.Limit is not null) query = query.Take(req.Limit.Value);

        var response = query
            .Select(item => item.ToPreviewResponse(isLithuanian, mediaFileStore))
            .OrderByDescending(item => item.CreatedDate)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
