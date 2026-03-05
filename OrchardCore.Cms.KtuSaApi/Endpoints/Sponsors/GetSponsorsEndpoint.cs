using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public class GetSponsorsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : EndpointWithoutRequest<List<SponsorResponse>>
{
    public override void Configure()
    {
        Get("api/sponsors");
        AllowAnonymous();
        Description(b => b
            .WithTags("Sponsors")
            .WithSummary("List sponsors")
            .WithDescription("Returns published sponsors ordered by creation date descending.")
            .Produces<List<SponsorResponse>>(200));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var sponsors = await repository.GetAllAsync(Sponsor);

        var response = sponsors
            .OrderByDescending(item => item.CreatedUtc)
            .Select(item => item.ToResponse(mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}