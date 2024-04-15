using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainContactsController(IRepository repository) : ControllerBase
{
    [HttpGet("{saUnit}")]
    [ProducesResponseType(typeof(MainContactDto), 200)]
    public async Task<ActionResult> GetMainContacts(SaUnit saUnit)
    {
        var contacts = await repository.GetAllAsync(MainContact);

        var filteredContact = contacts
            .Select(contact => contact)
            .FirstOrDefault(part => part.As<AddressPart>().SaUnit == saUnit.ToString());

        var addressPart = filteredContact.As<AddressPart>();
        var contactPart = filteredContact.As<ContactPart>();

        var contactDto = new MainContactDto
        {
            Address = addressPart.Address,
            Email = contactPart.Email,
            PhoneNumber = contactPart.PhoneNumber,
        };

        return Ok(contactDto);
    }
}