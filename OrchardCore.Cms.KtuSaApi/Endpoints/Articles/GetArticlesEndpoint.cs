using FastEndpoints;
using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetArticlesRequest, PagedResponse<ArticlePreviewResponse>>
{
    private const int DefaultPageSize = 9;

    public override void Configure()
    {
        Get("api/articles");
        AllowAnonymous();
        Description(b => b
            .WithTags("Articles")
            .WithSummary("List article previews")
            .WithDescription(
                "Returns a page of published article previews ordered by creation date descending. " +
                "Use query parameter language=en (default) or language=lt. " +
                "Use page and pageSize to page through the results; pageSize defaults to 9 and is capped at 100. " +
                "Optional query parameter limit caps the total number of articles considered before paging.")
            .Produces<PagedResponse<ArticlePreviewResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetArticlesRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Article);
        var language = req.Language;

        IEnumerable<ArticlePreviewResponse> articles = query
            .Select(item => item.ToPreviewResponse(language, mediaFileStore))
            .OrderByDescending(item => item.CreatedDate);

        if (req.Limit is not null) articles = articles.Take(req.Limit.Value);

        var response = PagedResponse.Create([.. articles], req.Page, req.PageSize, DefaultPageSize);

        await Send.OkAsync(response, ct);
    }
}
