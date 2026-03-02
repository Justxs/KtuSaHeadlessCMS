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
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return;

        var saUnits = await repository.GetAllAsync(ContentTypeConstants.SaUnit);

        builder.Add(T["General Info"], "1", content =>
        {
            content.AddClass("icon-class-fa-circle-info")
                .AddClass("icon-class-fas");

            foreach (var saUnit in saUnits)
            {
                var unitName = saUnit.As<SaUnitPart>().UnitName;

                content.Add(T[$"All {unitName.Replace("_", " ")} contacts"], type => type
                    .Url($"/Contacts/List/{unitName}")
                    .Permission(ContactPermissions
                        .GetPermission(Enum.Parse<SaUnit>(unitName, true)))
                    .AddClass("icon-class-fa-list")
                    .AddClass("icon-class-fas"));

                if (unitName is nameof(SaUnit.CSA) or nameof(SaUnit.BRK)) continue;

                content.Add(T["Edit " + unitName.Replace("_", " ") + " info"], "1", item => item
                    .Action("Edit", "Admin", new
                    {
                        area = "OrchardCore.Contents",
                        contentItemId = saUnit.ContentItemId
                    })
                    .Permission(SaUnitPermissions
                        .GetPermission(Enum.Parse<SaUnit>(unitName, true)))
                    .AddClass("icon-class-fa-pen-to-square")
                    .AddClass("icon-class-fas"));
            }
        });
    }
}