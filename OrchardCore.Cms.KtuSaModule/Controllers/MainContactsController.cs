using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainContactsController(IContentManager contentManager, ISession session) : ControllerBase
{
    private static readonly string MainContact = ContentTypeNames.MainContact.ToString();

    [HttpGet("{saUnit}")]
    public async Task<ActionResult> GetMainContacts(SaUnit saUnit)
    {
        var contacts = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == MainContact)
            .ListAsync();

        var filteredContact = new ContentItem();

        foreach (var contact in contacts)
        {
            await contentManager.LoadAsync(contact);
            var part = contact.As<AddressPart>();

            if (part.SaUnitSelectField != null && part.SaUnitSelectField.SaUnit == saUnit)
            {
                filteredContact = contact;
            }
        }

        var addressPart = filteredContact.As<AddressPart>();
        var contactPart = filteredContact.As<ContactPart>();

        var contactDto = new MainContactDto
        {
            Address = addressPart?.Address!,
            Email = contactPart?.Email!,
            PhoneNumber = contactPart?.PhoneNumber!,
        };

        return Ok(contactDto);
    }
}