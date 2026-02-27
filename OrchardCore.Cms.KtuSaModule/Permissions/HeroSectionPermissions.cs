using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class HeroSectionPermissions : IPermissionProvider
{
    public static readonly Permission ManageHeroSections =
        new(nameof(ManageHeroSections), "Can manage Hero sections for all pages");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(
            new[]
                {
                    ManageHeroSections
                }
                .AsEnumerable());
    }

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
    {
        return new[]
        {
            new PermissionStereotype
            {
                Name = Administrator,
                Permissions = new[] { ManageHeroSections }
            },
            new PermissionStereotype
            {
                Name = CsaEditor,
                Permissions = new[] { ManageHeroSections }
            }
        };
    }
}