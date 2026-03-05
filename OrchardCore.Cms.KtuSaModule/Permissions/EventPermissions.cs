using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Constants.RolesConstants;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class EventPermissions : SaUnitPermissionProvider
{
    public static readonly Permission ManageCsaEvents = new(nameof(ManageCsaEvents), "Can manage CSA events.");
    public static readonly Permission ManageBrkEvents = new(nameof(ManageBrkEvents), "Can manage BRK events.");
    public static readonly Permission ManageInfosaEvents = new(nameof(ManageInfosaEvents), "Can manage InfoSA events.");

    public static readonly Permission ManageVivatChemijaEvents =
        new(nameof(ManageVivatChemijaEvents), "Can manage Vivat chemija events.");

    public static readonly Permission ManageIndiEvents = new(nameof(ManageIndiEvents), "Can manage InDi events.");
    public static readonly Permission ManageEsaEvents = new(nameof(ManageEsaEvents), "Can manage ESA events.");
    public static readonly Permission ManageFumsaEvents = new(nameof(ManageFumsaEvents), "Can manage FUMSA events.");

    public static readonly Permission ManageStatiusEvents =
        new(nameof(ManageStatiusEvents), "Can manage STATIUS events.");

    public static readonly Permission ManageVfsaEvents = new(nameof(ManageVfsaEvents), "Can manage VFSA events.");
    public static readonly Permission ManageShmEvents = new(nameof(ManageShmEvents), "Can manage SHM events.");

    public static readonly Permission ManageEvents = new(nameof(ManageEvents), "Can manage events.",
    [
        ManageCsaEvents, ManageBrkEvents, ManageInfosaEvents, ManageVivatChemijaEvents, ManageIndiEvents,
        ManageEsaEvents, ManageFumsaEvents, ManageStatiusEvents, ManageVfsaEvents, ManageShmEvents
    ]);

    private static readonly Dictionary<SaUnit, Permission> _unitPermissions = new()
    {
        [SaUnit.CSA] = ManageCsaEvents,
        [SaUnit.BRK] = ManageBrkEvents,
        [SaUnit.InfoSA] = ManageInfosaEvents,
        [SaUnit.Vivat_Chemija] = ManageVivatChemijaEvents,
        [SaUnit.InDi] = ManageIndiEvents,
        [SaUnit.ESA] = ManageEsaEvents,
        [SaUnit.FUMSA] = ManageFumsaEvents,
        [SaUnit.STATIUS] = ManageStatiusEvents,
        [SaUnit.VFSA] = ManageVfsaEvents,
        [SaUnit.SHM] = ManageShmEvents
    };

    protected override IReadOnlyDictionary<SaUnit, Permission> UnitPermissions => _unitPermissions;

    protected override Permission[] AdditionalPermissions => [ManageEvents];

    protected override string[] FullAccessRoles => [Administrator, CsaEditor];
}