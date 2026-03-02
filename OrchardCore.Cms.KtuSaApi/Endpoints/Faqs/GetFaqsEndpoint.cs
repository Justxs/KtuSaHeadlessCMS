using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsEndpoint(IRepository repository)
    : Endpoint<GetFaqsRequest, List<FaqResponse>>
{
    public override void Configure()
    {
        Get("api/faqs");
        AllowAnonymous();
        ResponseCache(300);
        Description(b => b
            .WithTags("Faqs")
            .WithSummary("Get FAQ items")
            .WithDescription(
                "Returns a list of frequently asked questions in the specified language. " +
                "Pass language=lt or language=en. Query parameter 'limit' optionally returns a random subset.")
            .Produces<List<FaqResponse>>(200));
    }

    public override async Task HandleAsync(GetFaqsRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Faq);
        var language = req.Language;

        if (req.Limit is not null) query = query.OrderBy(_ => Guid.NewGuid()).Take(req.Limit.Value);

        var response = query.Select(item => item.ToFaqResponse(language))
            .OrderByDescending(item => item.ModifiedDate)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
