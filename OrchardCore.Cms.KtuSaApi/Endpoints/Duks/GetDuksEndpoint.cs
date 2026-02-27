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
        Get("api/{language}/Duks");
        AllowAnonymous();
        Description(b => b
            .WithTags("Duks")
            .WithSummary("Get DUK (FAQ) items")
            .WithDescription(
                "Returns a list of frequently asked questions in the specified language. Query parameter 'limit' optionally returns a random subset.")
            .Produces<List<DukResponse>>(200));
    }

    public override async Task HandleAsync(GetDuksRequest req, CancellationToken ct)
    {
        var duks = await repository.GetAllAsync(Duk);
        var isLithuanian = req.Language.IsLtLanguage();

        IEnumerable<ContentItem> query = duks.OrderByDescending(item => item.ModifiedUtc);

        if (req.Limit is not null) query = query.OrderBy(_ => Guid.NewGuid()).Take(req.Limit.Value);

        var dukDtos = query.Select(item => item.ToResponse(isLithuanian)).ToList();

        await Send.OkAsync(dukDtos, ct);
    }
}