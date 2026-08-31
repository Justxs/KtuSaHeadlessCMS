using FastEndpoints;
using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetEventsRequest, PagedResponse<EventPreviewResponse>>
{
    private const int DefaultPageSize = 9;

    public override void Configure()
    {
        Get("api/events");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("List event previews")
            .WithDescription(
                "Returns a page of published event previews ordered by start date descending. " +
                "Use query parameter language=en (default) or language=lt. " +
                "Optionally filter results by saUnit. " +
                "Use page and pageSize to page through the results; pageSize defaults to 9 and is capped at 100.")
            .Produces<PagedResponse<EventPreviewResponse>>(200)
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
                query = query.Where(item =>
                item.GetOrCreate<EventPart>().OrganisersField.ContentItemIds.Contains(saUnit.ContentItemId));
        }

        var events = query
            .OrderByDescending(item => item.GetOrCreate<EventPart>().StartDate)
            .Select(item => item.ToPreviewResponse(language, mediaFileStore))
            .ToList();

        var response = PagedResponse.Create(events, req.Page, req.PageSize, DefaultPageSize);

        await Send.OkAsync(response, ct);
    }
}
