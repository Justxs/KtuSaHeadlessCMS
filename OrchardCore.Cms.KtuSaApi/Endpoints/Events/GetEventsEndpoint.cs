using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsEndpoint(IRepository repository)
    : Endpoint<GetEventsRequest, List<EventPreviewResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/Events");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("Get all events")
            .WithDescription(
                "Returns a list of event previews ordered by start date descending. Language: 'lt' or 'en'. Pass fetchPassed=true to include past events.")
            .Produces<List<EventPreviewResponse>>(200));
    }

    public override async Task HandleAsync(GetEventsRequest req, CancellationToken ct)
    {
        var events = await repository.GetAllAsync(Event);
        var isLithuanian = req.Language.IsLtLanguage();

        var eventDtos = events
            .OrderByDescending(item => item.As<EventPart>().StartDate)
            .Select(item => item.ToPreviewResponse(isLithuanian))
            .ToList();

        await Send.OkAsync(eventDtos, ct);
    }
}