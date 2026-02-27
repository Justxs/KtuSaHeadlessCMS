using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticlesEndpoint(IRepository repository)
    : Endpoint<GetArticlesRequest, List<ArticlePreviewResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/Articles");
        AllowAnonymous();
        Description(b => b
            .WithTags("Articles")
            .WithSummary("Get all articles")
            .WithDescription(
                "Returns a list of article previews ordered by creation date descending. Language: 'lt' or 'en'. Query parameter 'limit' optionally caps the number of results.")
            .Produces<List<ArticlePreviewResponse>>(200));
    }

    public override async Task HandleAsync(GetArticlesRequest req, CancellationToken ct)
    {
        var articles = await repository.GetAllAsync(Article);
        var isLithuanian = req.Language.IsLtLanguage();

        IEnumerable<ContentItem> query = articles.OrderByDescending(item => item.CreatedUtc);

        if (req.Limit is not null) query = query.Take(req.Limit.Value);

        var articleDtos = query.Select(item => item.ToPreviewResponse(isLithuanian)).ToList();

        await Send.OkAsync(articleDtos, ct);
    }
}