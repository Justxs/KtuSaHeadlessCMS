using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ArticlePermissions : IPermissionProvider
{
    public static readonly Permission ManageArticles = new(nameof(ManageArticles), "Can manage articles content.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
            {
                ManageArticles
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
                Permissions = new[] { ManageArticles }
            },
            new PermissionStereotype
            {
                Name = CsaEditor,
                Permissions = new[] { ManageArticles }
            }
        };
    }
}