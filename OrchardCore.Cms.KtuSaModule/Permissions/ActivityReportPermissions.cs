using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ActivityReportPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageActivityReports =
        new(nameof(ManageActivityReports), "Can manage activity reports.");

    protected override Permission Permission => ManageActivityReports;

    protected override string[] Roles => [Administrator, CsaEditor];
}