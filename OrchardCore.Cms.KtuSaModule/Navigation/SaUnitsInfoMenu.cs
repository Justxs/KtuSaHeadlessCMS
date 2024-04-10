using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Navigation;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class SaUnitsInfoMenu(
    IStringLocalizer<AdminMenu> stringLocalizer,
    IHttpContextAccessor httpContextAccessor,
    IRepository repository)
    : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public async Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return;
        }

        var saUnits = await repository.GetAllAsync(nameof(ContentTypeNames.SaUnit));

        builder.Add(T["General Info"], "1", content =>
        {
            content.AddClass("icon-class-fa-circle-info")
                .AddClass("icon-class-fas");

            foreach (var saUnit in saUnits)
            {
                var unitName = saUnit.As<SaUnitPart>().UnitName;
                if (unitName is nameof(SaUnit.CSA) or nameof(SaUnit.BRK))
                {
                    continue;
                }

                content.Add(T[unitName.Replace("_", "")], "1", item => item
                    .Action("Edit", "Admin", new
                    {
                        area = "OrchardCore.Contents", 
                        contentItemId = saUnit.ContentItemId,
                    })
                    .Permission(SaUnitPermissions
                        .GetPermission((SaUnit)Enum.Parse(typeof(SaUnit), unitName, true)))
                    .AddClass("icon-class-fa-pen-to-square")
                    .AddClass("icon-class-fas"));
            }
        });
    }
}