using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetEventsRequest, List<EventPreviewResponse>>
{
    public override void Configure()
    {
        Get("api/events");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("Get events")
            .WithDescription(
                "Returns a list of event previews ordered by start date descending. " +
                "Pass language=lt or language=en. Pass fetchPassed=true to include past events. " +
                "Optionally filter by saUnit (CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK).")
            .Produces<List<EventPreviewResponse>>(200));
    }

    public override async Task HandleAsync(GetEventsRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Event);
        var isLithuanian = req.Language.IsLtLanguage();

        if (req.SaUnit is not null)
        {
            var saUnit = await repository.GetSaUnitByNameAsync(req.SaUnit.Value);
            if (saUnit is not null)
            {
                query = query.Where(item =>
                    item.As<EventPart>().OrganisersField.ContentItemIds.Contains(saUnit.ContentItemId));
            }
        }

        var response = query
            .OrderByDescending(item => item.As<EventPart>().StartDate)
            .Select(item => item.ToPreviewResponse(isLithuanian, mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
