using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Sponsors;

public class GetSponsorsEndpoint(IRepository repository)
    : EndpointWithoutRequest<List<SponsorResponse>>
{
    public override void Configure()
    {
        Get("api/Sponsors");
        AllowAnonymous();
        Description(b => b
            .WithTags("Sponsors")
            .WithSummary("Get all sponsors")
            .WithDescription("Returns a list of all sponsors ordered by creation date descending.")
            .Produces<List<SponsorResponse>>(200));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var sponsors = await repository.GetAllAsync(Sponsor);

        var sponsorDtos = sponsors
            .OrderByDescending(item => item.CreatedUtc)
            .Select(item => item.ToResponse())
            .ToList();

        await Send.OkAsync(sponsorDtos, ct);
    }
}