using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class GetMainContactsEndpoint(IRepository repository)
    : Endpoint<GetMainContactsRequest, MainContactResponse>
{
    public override void Configure()
    {
        Get("api/MainContacts/{saUnit}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Main Contacts")
            .WithSummary("Get main contact for an SA unit")
            .WithDescription("Returns the primary address and contact details for a specific student association unit. Allowed saUnit values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")
            .Produces<MainContactResponse>(200));
    }

    public override async Task HandleAsync(GetMainContactsRequest req, CancellationToken ct)
    {
        var contacts = await repository.GetAllAsync(MainContact);

        var filteredContact = contacts
            .FirstOrDefault(part => part.As<AddressPart>().SaUnit == req.SaUnit.ToString());

        var addressPart = filteredContact.As<AddressPart>();
        var contactPart = filteredContact.As<ContactPart>();

        var contactDto = new MainContactResponse
        {
            Address = addressPart.Address,
            Email = contactPart.Email,
            PhoneNumber = contactPart.PhoneNumber,
        };

        await Send.OkAsync(contactDto, ct);
    }
}
