using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class FaqPermissions : SimplePermissionProvider
{
    public static readonly Permission ManageFaqs = new(nameof(ManageFaqs), "Can manage FAQ content.");

    protected override Permission Permission => ManageFaqs;

    protected override string[] Roles => [Administrator, CsaEditor];
}
