using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventsBySaUnitEndpoint(IRepository repository)
    : Endpoint<GetEventsBySaUnitRequest, List<EventPreviewResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/Events/SaUnits/{saUnit}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("Get events by SA unit")
            .WithDescription(
                "Returns event previews organised by the specified SA unit, ordered by start date descending. Language: 'lt' or 'en'. Pass fetchPassed=true to include past events. Allowed saUnit values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")
            .Produces<List<EventPreviewResponse>>(200));
    }

    public override async Task HandleAsync(GetEventsBySaUnitRequest req, CancellationToken ct)
    {
        var events = await repository.GetAllAsync(Event);
        var saUnit = await repository.GetSaUnitByName(req.SaUnit);
        var isLithuanian = req.Language.IsLtLanguage();

        var eventDtos = events
            .Where(item => item.As<EventPart>().OrganisersField.ContentItemIds.Contains(saUnit.ContentItemId))
            .OrderByDescending(item => item.As<EventPart>().StartDate)
            .Select(item => item.ToPreviewResponse(isLithuanian))
            .ToList();

        await Send.OkAsync(eventDtos, ct);
    }
}