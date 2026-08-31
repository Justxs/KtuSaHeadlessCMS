using FastEndpoints;
using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetFaqsRequest, PagedResponse<FaqResponse>>
{
    private const int DefaultPageSize = 10;

    public override void Configure()
    {
        Get("api/faqs");
        AllowAnonymous();
        Description(b => b
            .WithTags("Faqs")
            .WithSummary("Get FAQ items")
            .WithDescription(
                "Returns a page of published frequently asked questions ordered by modification date descending. " +
                "Use query parameter language=en (default) or language=lt. " +
                "Use search to filter by question or answer text. " +
                "Use page and pageSize to page through the results; pageSize defaults to 10 and is capped at 100. " +
                "Optional query parameter limit returns a random subset instead, ignoring search and paging.")
            .Produces<PagedResponse<FaqResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetFaqsRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Faq);
        var language = req.Language;

        if (req.Limit is not null)
        {
            var sample = query
                .OrderBy(_ => Guid.NewGuid())
                .Take(req.Limit.Value)
                .Select(item => item.ToFaqResponse(language, mediaFileStore))
                .OrderByDescending(item => item.ModifiedDate)
                .ToList();

            await Send.OkAsync(
                PagedResponse.Create(sample, 1, Math.Max(sample.Count, 1), DefaultPageSize),
                ct);
            return;
        }

        IEnumerable<FaqResponse> faqs = query
            .Select(item => item.ToFaqResponse(language, mediaFileStore))
            .OrderByDescending(item => item.ModifiedDate);

        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            var search = req.Search.Trim();
            faqs = faqs.Where(faq => Matches(faq, search));
        }

        var response = PagedResponse.Create([.. faqs], req.Page, req.PageSize, DefaultPageSize);

        await Send.OkAsync(response, ct);
    }

    private static bool Matches(FaqResponse faq, string search)
    {
        if (faq.Question.Contains(search, StringComparison.OrdinalIgnoreCase)) return true;

        return faq.Answer
            .Select(block => block.Html)
            .Where(html => !string.IsNullOrWhiteSpace(html))
            .Select(html => HtmlTagRemoveRegex().Replace(html!, " "))
            .Any(text => text.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
}
