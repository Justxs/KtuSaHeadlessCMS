using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class BackupPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageBackups = new(
        nameof(ManageBackups),
        "Can download the database and upload a backup to restore it.");

    protected override Permission Permission => ManageBackups;

    // Deliberately Administrator only: the download contains every user record,
    // and a restore replaces the entire site.
    protected override string[] Roles => [Administrator];
}
