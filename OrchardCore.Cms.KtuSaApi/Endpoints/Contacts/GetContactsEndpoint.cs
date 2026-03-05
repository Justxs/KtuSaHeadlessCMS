using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class GetContactsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetContactsRequest, List<ContactResponse>>
{
    public override void Configure()
    {
        Get("api/sa-units/{saUnit}/contacts");
        AllowAnonymous();
        Description(b => b
            .WithTags("Contacts")
            .WithSummary("List contacts for an SA unit")
            .WithDescription(
                "Returns published member contacts for the specified SA unit ordered by index (ascending). " +
                "Use query parameter language=en (default) or language=lt. If the SA unit does not exist, an empty array is returned.")
            .Produces<List<ContactResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetContactsRequest req, CancellationToken ct)
    {
        var saUnit = await repository.GetSaUnitByNameAsync(req.SaUnit);

        if (saUnit is null)
        {
            await Send.OkAsync([], ct);
            return;
        }

        var contacts = await repository.GetAllAsync(Contact);
        var positions = await repository.GetAllAsync(Position);
        var language = req.Language;

        var response = contacts
            .Where(item => item.As<MemberPart>().SaUnit.ContentItemIds.Contains(saUnit.ContentItemId))
            .Select(item => item.ToResponse(language, positions, mediaFileStore))
            .OrderBy(contact => contact.Index)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
