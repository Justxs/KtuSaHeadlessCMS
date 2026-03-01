using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public abstract class SimplePermissionProvider : IPermissionProvider
{
    protected abstract Permission Permission { get; }

    protected abstract string[] Roles { get; }

    public Task<IEnumerable<Permission>> GetPermissionsAsync() =>
        Task.FromResult<IEnumerable<Permission>>([Permission]);

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
        Roles.Select(role => new PermissionStereotype { Name = role, Permissions = [Permission] });
}
