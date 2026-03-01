using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ArticlePermissions : SimplePermissionProvider
{
    public static readonly Permission ManageArticles = new(nameof(ManageArticles), "Can manage articles content.");

    protected override Permission Permission => ManageArticles;

    protected override string[] Roles => [Administrator, CsaEditor];
}