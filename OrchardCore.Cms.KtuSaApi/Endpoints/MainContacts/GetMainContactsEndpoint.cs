using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class GetMainContactsEndpoint(IRepository repository)
    : Endpoint<GetMainContactsRequest, MainContactResponse>
{
    public override void Configure()
    {
        Get("api/sa-units/{saUnit}/main-contact");
        AllowAnonymous();
        Description(b => b
            .WithTags("Main Contacts")
            .WithSummary("Get main contact for an SA unit")
            .WithDescription(
                "Returns the primary address and contact details for a specific student association unit.")
            .Produces<MainContactResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetMainContactsRequest req, CancellationToken ct)
    {
        var contacts = await repository.GetAllAsync(MainContact);

        var contact = contacts.FirstOrDefault(item => item.As<AddressPart>().SaUnit == req.SaUnit.ToString());

        if (contact is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(contact.ToResponse(), ct);
    }
}