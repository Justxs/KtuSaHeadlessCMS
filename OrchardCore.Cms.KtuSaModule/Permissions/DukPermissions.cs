using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Models.Enums.Roles;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class DukPermissions : IPermissionProvider
{
    public static readonly Permission ManageDuks = new(nameof(ManageDuks), "Can manage DUK content.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
            {
                ManageDuks,
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
                Permissions = new[] { ManageDuks },
            },
            new PermissionStereotype
            {
                Name = CsaEditor.ToString(),
                Permissions = new[] {
                    ManageDuks,
                },
            },
        };
    }
}