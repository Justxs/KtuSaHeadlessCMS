using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class DocumentsPermissions : IPermissionProvider
{
    public static readonly Permission ManageDocuments = new(nameof(ManageDocuments), "Can manage Documents content.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
        {
            ManageDocuments,
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
                Permissions = new[] { ManageDocuments },
            },
            new PermissionStereotype
            {
                Name = CsaEditor,
                Permissions = new[] {
                    ManageDocuments,
                },
            },
        };
    }
}