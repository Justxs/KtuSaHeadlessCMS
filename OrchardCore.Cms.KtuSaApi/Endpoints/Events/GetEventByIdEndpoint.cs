using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdEndpoint(IContentManager contentManager)
    : Endpoint<GetEventByIdRequest, EventContentResponse>
{
    public override void Configure()
    {
        Get("api/{language}/Events/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Events")
            .WithSummary("Get an event by ID")
            .WithDescription("Returns the full content of an event including HTML body, dates, address, Fienta ticket URL and organising SA units. Language: 'lt' or 'en'.")
            .Produces<EventContentResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetEventByIdRequest req, CancellationToken ct)
    {
        var eventItem = await contentManager.GetAsync(req.Id);

        if (eventItem is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var isLithuanian = req.Language.IsLtLanguage();

        var part = eventItem.As<EventPart>();
        var saUnits = await contentManager.GetAsync(part.OrganisersField.ContentItemIds);
        var organisers = saUnits.Select(unit => unit.As<SaUnitPart>().UnitName).ToList();

        var eventDto = new EventContentResponse
        {
            Id = eventItem.ContentItemId,
            Title = isLithuanian ? part.TitleLt : part.TitleEn,
            HtmlBody = isLithuanian ? part.BodyFieldLt.HtmlBody : part.BodyFieldEn.HtmlBody,
            FientaTicketUrl = GetFientaUrl(isLithuanian ? part.FientaTicketLinkLt : part.FientaTicketLinkEn, isLithuanian),
            StartDate = part.StartDate,
            EndDate = part.EndDate,
            Address = part.Address,
            FacebookUrl = part.FbEventLink,
            CoverImageUrl = part.ImageUploadField.FileId,
            Organisers = organisers,
        };

        await Send.OkAsync(eventDto, ct);
    }

    private static string? GetFientaUrl(string? raw, bool isLithuanian)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var parts = raw.Split("|||", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => null,
            1 => parts[0],
            _ => isLithuanian ? parts[0] : parts[^1]
        };
    }
}
