using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetEventByIdRequest, EventContentResponse>
{
    public override void Configure()
    {
        Get("api/events/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("Get an event by ID")
            .WithDescription(
                "Returns a published event by content item ID, including structured content blocks, dates, address, " +
                "ticket link, and organizer SA units. Use query parameter language=en (default) or language=lt.")
            .Produces<EventContentResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetEventByIdRequest req, CancellationToken ct)
    {
        var eventItem = await repository.GetByIdAsync(req.Id);

        if (eventItem is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var language = req.Language;
        var part = eventItem.As<EventPart>();
        var saUnits = await repository.GetByIdsAsync(part.OrganisersField.ContentItemIds);
        var organisers = saUnits.Select(unit => unit.As<SaUnitPart>().UnitName).ToList();

        await Send.OkAsync(eventItem.ToContentResponse(language, organisers, mediaFileStore), ct);
    }
}