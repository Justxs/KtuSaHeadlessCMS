using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class SponsorPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageSponsors = new(nameof(ManageSponsors), "Can manage Sponsors content.");

    protected override Permission Permission => ManageSponsors;

    protected override string[] Roles => [Administrator, Marketing];
}