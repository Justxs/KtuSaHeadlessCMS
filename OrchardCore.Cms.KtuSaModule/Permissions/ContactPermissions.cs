using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ContactPermissions : SaUnitPermissionProvider
{
    public static readonly Permission ManageCsaContacts = new(nameof(ManageCsaContacts), "Can manage CSA contacts.");
    public static readonly Permission ManageBrkContacts = new(nameof(ManageBrkContacts), "Can manage BRK contacts.");

    public static readonly Permission ManageInfosaContacts =
        new(nameof(ManageInfosaContacts), "Can manage InfoSA contacts.");

    public static readonly Permission ManageVivatChemijaContacts =
        new(nameof(ManageVivatChemijaContacts), "Can manage Vivat chemija contacts.");

    public static readonly Permission ManageIndiContacts = new(nameof(ManageIndiContacts), "Can manage InDi contacts.");
    public static readonly Permission ManageEsaContacts = new(nameof(ManageEsaContacts), "Can manage ESA contacts.");

    public static readonly Permission ManageFumsaContacts =
        new(nameof(ManageFumsaContacts), "Can manage FUMSA contacts.");

    public static readonly Permission ManageStatiusContacts =
        new(nameof(ManageStatiusContacts), "Can manage STATIUS contacts.");

    public static readonly Permission ManageVfsaContacts = new(nameof(ManageVfsaContacts), "Can manage VFSA contacts.");
    public static readonly Permission ManageShmContacts = new(nameof(ManageShmContacts), "Can manage SHM contacts.");
    public static readonly Permission ManagePositions = new(nameof(ManagePositions), "Can manage KTU SA positions.");

    private static readonly Dictionary<SaUnit, Permission> _unitPermissions = new()
    {
        [SaUnit.CSA] = ManageCsaContacts,
        [SaUnit.BRK] = ManageBrkContacts,
        [SaUnit.InfoSA] = ManageInfosaContacts,
        [SaUnit.Vivat_Chemija] = ManageVivatChemijaContacts,
        [SaUnit.InDi] = ManageIndiContacts,
        [SaUnit.ESA] = ManageEsaContacts,
        [SaUnit.FUMSA] = ManageFumsaContacts,
        [SaUnit.STATIUS] = ManageStatiusContacts,
        [SaUnit.VFSA] = ManageVfsaContacts,
        [SaUnit.SHM] = ManageShmContacts
    };

    protected override IReadOnlyDictionary<SaUnit, Permission> UnitPermissions => _unitPermissions;

    protected override Permission[] AdditionalPermissions => [ManagePositions];

    protected override string[] FullAccessRoles => [Administrator, CsaEditor];

    protected override IEnumerable<PermissionStereotype> ExtraStereotypes =>
    [
        new() { Name = President, Permissions = [ManagePositions] }
    ];

    public static Permission GetPermission(SaUnit unit)
    {
        return _unitPermissions[unit];
    }
}