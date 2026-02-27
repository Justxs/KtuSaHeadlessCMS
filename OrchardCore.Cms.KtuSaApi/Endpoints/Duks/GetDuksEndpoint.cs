using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Duks;

public class GetDuksEndpoint(IRepository repository)
    : Endpoint<GetDuksRequest, List<DukResponse>>
{
    public override void Configure()
    {
        Get("api/duks");
        AllowAnonymous();
        Description(b => b
            .WithTags("Faqs")
            .WithSummary("Get FAQ items")
            .WithDescription(
                "Returns a list of frequently asked questions in the specified language. " +
                "Pass language=lt or language=en. Query parameter 'limit' optionally returns a random subset.")
            .Produces<List<DukResponse>>(200));
    }

    public override async Task HandleAsync(GetDuksRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Duk);
        var isLithuanian = req.Language.IsLtLanguage();

        if (req.Limit is not null) query = query.OrderBy(_ => Guid.NewGuid()).Take(req.Limit.Value);

        var response = query.Select(item => item.ToResponse(isLithuanian))
            .OrderByDescending(item => item.ModifiedDate)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}