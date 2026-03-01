using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public abstract class SaUnitPermissionProvider : IPermissionProvider
{
    protected abstract IReadOnlyDictionary<SaUnit, Permission> UnitPermissions { get; }

    protected virtual Permission[] AdditionalPermissions => [];

    protected virtual string[] FullAccessRoles => [Administrator];

    protected virtual IEnumerable<PermissionStereotype> ExtraStereotypes => [];

    public Task<IEnumerable<Permission>> GetPermissionsAsync() =>
        Task.FromResult(UnitPermissions.Values
            .Concat(AdditionalPermissions)
            .AsEnumerable());

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
    {
        var allUnitPermissions = UnitPermissions.Values.ToArray();

        foreach (var role in FullAccessRoles)
        {
            yield return new PermissionStereotype { Name = role, Permissions = allUnitPermissions };
        }

        foreach (var stereotype in ExtraStereotypes)
        {
            yield return stereotype;
        }

        var skipRoles = FullAccessRoles
            .Concat(ExtraStereotypes.Select(s => s.Name))
            .ToHashSet();

        foreach (var (unit, permission) in UnitPermissions)
        {
            var editorRole = GetEditorRole(unit);
            if (!skipRoles.Contains(editorRole))
            {
                yield return new PermissionStereotype { Name = editorRole, Permissions = [permission] };
            }
        }
    }
}
