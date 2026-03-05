using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class GetFaqsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetFaqsRequest, List<FaqResponse>>
{
    public override void Configure()
    {
        Get("api/faqs");
        AllowAnonymous();
        Description(b => b
            .WithTags("Faqs")
            .WithSummary("Get FAQ items")
            .WithDescription(
                "Returns published frequently asked questions. " +
                "Use query parameter language=en (default) or language=lt. " +
                "Optional query parameter limit returns a random subset up to the specified count.")
            .Produces<List<FaqResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetFaqsRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Faq);
        var language = req.Language;

        if (req.Limit is not null) query = query.OrderBy(_ => Guid.NewGuid()).Take(req.Limit.Value);

        var response = query.Select(item => item.ToFaqResponse(language, mediaFileStore))
            .OrderByDescending(item => item.ModifiedDate)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}