using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
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
        ResponseCache(300);
        Description(b => b
            .WithTags("Contacts")
            .WithSummary("Get contacts for an SA unit")
            .WithDescription(
                "Returns member contacts of the specified SA unit ordered by index. " +
                "Includes name, email, photo, position and responsibilities. " +
                "Pass language=lt or language=en.")
            .Produces<List<ContactResponse>>(200));
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
        var isLithuanian = req.Language.IsLtLanguage();

        var response = contacts
            .Where(item => item.As<MemberPart>().SaUnit.ContentItemIds.Contains(saUnit.ContentItemId))
            .Select(item => item.ToResponse(isLithuanian, positions, mediaFileStore))
            .OrderBy(contact => contact.Index)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
