using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class HeroSectionPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageHeroSections =
        new(nameof(ManageHeroSections), "Can manage Hero sections for all pages");

    protected override Permission Permission => ManageHeroSections;

    protected override string[] Roles => [Administrator, CsaEditor];
}