using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
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
                "Returns details for a specific student association unit, including social media links, description and contact information. " +
                "Pass language=lt or language=en.")
            .Produces<SaUnitResponse>(200)
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

        var isLithuanian = req.Language.IsLtLanguage();

        await Send.OkAsync(saUnit.ToResponse(isLithuanian, mediaFileStore), ct);
    }
}
