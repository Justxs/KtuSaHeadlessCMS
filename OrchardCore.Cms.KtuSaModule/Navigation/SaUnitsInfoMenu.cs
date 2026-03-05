using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Navigation;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Constants;
using SaUnit = OrchardCore.Cms.KtuSaModule.Models.Enums.SaUnit;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class SaUnitsInfoMenu(
    IStringLocalizer<SaUnitsInfoMenu> stringLocalizer,
    IRepository repository)
    : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public async ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!name.IsAdminMenu()) return;

        var saUnits = await repository.GetAllAsync(ContentTypeConstants.SaUnit);

        builder.Add(T["General Info"], "1", content =>
        {
            content.WithIcon("icon-class-fa-circle-info");

            foreach (var saUnit in saUnits)
            {
                var unitName = saUnit.As<SaUnitPart>().UnitName;
                var parsedSaUnit = Enum.Parse<SaUnit>(unitName, true);
                var unitDisplayName = unitName.Replace("_", " ");

                content.Add(T[$"All {unitDisplayName} contacts"], type => type
                    .Url($"/Contacts/List/{unitName}")
                    .Permission(ContactPermissions.GetPermission(parsedSaUnit))
                    .WithIcon("icon-class-fa-list"));

                if (parsedSaUnit is SaUnit.CSA or SaUnit.BRK) continue;

                content.Add(T[$"Edit {unitDisplayName} info"], "1", item => item
                    .Action("Edit", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentItemId = saUnit.ContentItemId
                    })
                    .Permission(SaUnitPermissions.GetPermission(parsedSaUnit))
                    .WithIcon("icon-class-fa-pen-to-square"));
            }
        });
    }
}
