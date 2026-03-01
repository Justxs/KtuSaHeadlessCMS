using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class DocumentsPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageDocuments = new(nameof(ManageDocuments), "Can manage Documents content.");

    protected override Permission Permission => ManageDocuments;

    protected override string[] Roles => [Administrator, CsaEditor];
}