using FastEndpoints;
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
            .WithSummary("List event previews")
            .WithDescription(
                "Returns published event previews ordered by start date descending. " +
                "Use query parameter language=en (default) or language=lt. " +
                "Optionally filter results by saUnit.")
            .Produces<List<EventPreviewResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetEventsRequest req, CancellationToken ct)
    {
        var query = await repository.GetAllAsync(Event);
        var language = req.Language;

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
            .Select(item => item.ToPreviewResponse(language, mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
