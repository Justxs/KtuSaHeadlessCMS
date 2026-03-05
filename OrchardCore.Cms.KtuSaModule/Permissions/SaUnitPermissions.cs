using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class SaUnitPermissions : SaUnitPermissionProvider
{
    public static readonly Permission ManageCsaInfo = new(nameof(ManageCsaInfo), "Can manage CSA info.");
    public static readonly Permission ManageBrkInfo = new(nameof(ManageBrkInfo), "Can manage BRK info.");
    public static readonly Permission ManageInfosaInfo = new(nameof(ManageInfosaInfo), "Can manage InfoSA info.");

    public static readonly Permission ManageVivatChemijaInfo =
        new(nameof(ManageVivatChemijaInfo), "Can manage Vivat chemija info.");

    public static readonly Permission ManageIndiInfo = new(nameof(ManageIndiInfo), "Can manage InDi info.");
    public static readonly Permission ManageEsaInfo = new(nameof(ManageEsaInfo), "Can manage ESA info.");
    public static readonly Permission ManageFumsaInfo = new(nameof(ManageFumsaInfo), "Can manage FUMSA info.");
    public static readonly Permission ManageStatiusInfo = new(nameof(ManageStatiusInfo), "Can manage STATIUS info.");
    public static readonly Permission ManageVfsaInfo = new(nameof(ManageVfsaInfo), "Can manage VFSA info.");
    public static readonly Permission ManageShmInfo = new(nameof(ManageShmInfo), "Can manage SHM info.");

    private static readonly Dictionary<SaUnit, Permission> _unitPermissions = new()
    {
        [SaUnit.CSA] = ManageCsaInfo,
        [SaUnit.BRK] = ManageBrkInfo,
        [SaUnit.InfoSA] = ManageInfosaInfo,
        [SaUnit.Vivat_Chemija] = ManageVivatChemijaInfo,
        [SaUnit.InDi] = ManageIndiInfo,
        [SaUnit.ESA] = ManageEsaInfo,
        [SaUnit.FUMSA] = ManageFumsaInfo,
        [SaUnit.STATIUS] = ManageStatiusInfo,
        [SaUnit.VFSA] = ManageVfsaInfo,
        [SaUnit.SHM] = ManageShmInfo
    };

    protected override IReadOnlyDictionary<SaUnit, Permission> UnitPermissions => _unitPermissions;

    protected override IEnumerable<PermissionStereotype> ExtraStereotypes =>
    [
        new() { Name = President, Permissions = [ManageCsaInfo] }
    ];

    public static Permission GetPermission(SaUnit unit)
    {
        return _unitPermissions[unit];
    }
}