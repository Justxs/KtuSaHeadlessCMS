using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class SponsorPermissions : IPermissionProvider
{
    public static readonly Permission ManageSponsors = new(nameof(ManageSponsors), "Can manage Sponsors content.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
            {
                ManageSponsors,
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
                Permissions = new[] { ManageSponsors },
            },
        };
    }
}