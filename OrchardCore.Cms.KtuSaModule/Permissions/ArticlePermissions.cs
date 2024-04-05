using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Models.Enums.Roles;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ArticlePermissions : IPermissionProvider
{
    public static readonly Permission ManageArticles = new(nameof(ManageArticles), "Can manage articles content.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
            {
                ManageArticles,
            }
            .AsEnumerable());
    }

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
    {
        return new[]
        {
            new PermissionStereotype
            {
                Name = Administrator.ToString(),
                Permissions = new[] { ManageArticles },
            },
            new PermissionStereotype
            {
                Name = CsaEditor.ToString(),
                Permissions = new[] { ManageArticles },
            },
        };
    }
}