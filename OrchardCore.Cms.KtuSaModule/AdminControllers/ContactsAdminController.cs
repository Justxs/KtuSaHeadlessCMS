using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.AdminControllers;

[Admin]
public class ContactsAdminController(
    IRepository repository,
    IContentItemDisplayManager contentItemDisplayManager,
    IUpdateModelAccessor updateModelAccessor,
    IAuthorizationService authorizationService) : Controller
{
    [HttpGet]
    [Route("Contacts/List/{saUnit}")]
    public async Task<IActionResult> ListContacts(SaUnit saUnit)
    {
        if (!Enum.IsDefined(saUnit)) return NotFound();

        if (!await authorizationService.AuthorizeAsync(User, ContactPermissions.GetPermission(saUnit)))
            return Forbid();

        var contacts = await repository.GetAllAsync(Contact);
        var saUnitItem = await repository.GetSaUnitByNameAsync(saUnit);

        if (saUnitItem is null) return NotFound();

        contacts = contacts.Where(c => c.GetOrCreate<MemberPart>().SaUnit.ContentItemIds.Contains(saUnitItem.ContentItemId));

        var shapes = new List<IShape>();

        foreach (var item in contacts)
        {
            var shape = await contentItemDisplayManager.BuildDisplayAsync(item, updateModelAccessor.ModelUpdater,
                "SummaryAdmin");
            shapes.Add(shape);
        }

        return View(shapes);
    }
}
