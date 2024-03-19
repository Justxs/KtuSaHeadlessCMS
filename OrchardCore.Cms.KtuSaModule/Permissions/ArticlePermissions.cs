using OrchardCore.Security.Permissions;

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
                Name = "Administrator",
                Permissions = new[] { ManageArticles },
            },
        };
    }
}