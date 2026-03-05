using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class GetSaUnitEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetSaUnitRequest, SaUnitResponse>
{
    public override void Configure()
    {
        Get("api/sa-units/{saUnit}");
        AllowAnonymous();
        Description(b => b
            .WithTags("SA Units")
            .WithSummary("Get SA unit details")
            .WithDescription(
                "Returns SA unit details for the specified unit code, including cover image, structured body content, " +
                "contact data, and social links. Use query parameter language=en (default) or language=lt.")
            .Produces<SaUnitResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetSaUnitRequest req, CancellationToken ct)
    {
        var saUnit = await repository.GetSaUnitByNameAsync(req.SaUnit);

        if (saUnit is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var language = req.Language;

        await Send.OkAsync(saUnit.ToResponse(language, mediaFileStore), ct);
    }
}