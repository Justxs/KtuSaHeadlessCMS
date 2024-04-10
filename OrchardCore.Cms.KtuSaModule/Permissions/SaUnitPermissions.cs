using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Models.Enums.Roles;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class SaUnitPermissions : IPermissionProvider
{
    public static readonly Permission ManageCsaInfo = new(nameof(ManageCsaInfo), "Can manage CSA info.");
    public static readonly Permission ManageBrkInfo = new(nameof(ManageBrkInfo), "Can manage BRK info.");
    public static readonly Permission ManageInfosaInfo = new(nameof(ManageInfosaInfo), "Can manage InfoSA info.");
    public static readonly Permission ManageVivatChemijaInfo = new(nameof(ManageVivatChemijaInfo), "Can manage Vivat chemija info.");
    public static readonly Permission ManageIndiInfo = new(nameof(ManageIndiInfo), "Can manage InDi info.");
    public static readonly Permission ManageEsaInfo = new(nameof(ManageEsaInfo), "Can manage ESA info.");
    public static readonly Permission ManageFumsaInfo = new(nameof(ManageFumsaInfo), "Can manage FUMSA info.");
    public static readonly Permission ManageStatiusInfo = new(nameof(ManageStatiusInfo), "Can manage STATIUS info.");
    public static readonly Permission ManageVfsaInfo = new(nameof(ManageVfsaInfo), "Can manage VFSA info.");
    public static readonly Permission ManageShmInfo = new(nameof(ManageShmInfo), "Can manage SHM info.");


    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
        {
            ManageCsaInfo,
            ManageBrkInfo,
            ManageInfosaInfo,
            ManageVivatChemijaInfo,
            ManageIndiInfo,
            ManageEsaInfo,
            ManageFumsaInfo,
            ManageStatiusInfo,
            ManageVfsaInfo,
            ManageShmInfo,
        }
        .AsEnumerable());
    }

    public static Permission GetPermission(SaUnit saUnit)
    {
        var permission = saUnit switch
        {
            SaUnit.CSA => ManageCsaInfo,
            SaUnit.InfoSA => ManageInfosaInfo,
            SaUnit.Vivat_Chemija => ManageVivatChemijaInfo,
            SaUnit.InDi => ManageIndiInfo,
            SaUnit.STATIUS => ManageStatiusInfo,
            SaUnit.FUMSA => ManageFumsaInfo,
            SaUnit.ESA => ManageEsaInfo,
            SaUnit.SHM => ManageShmInfo,
            SaUnit.VFSA => ManageVfsaInfo,
            SaUnit.BRK => ManageBrkInfo,
            _ => throw new ArgumentException("Invalid SaUnit", nameof(saUnit)),
        };

        return permission;
    }

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
    {
        return new[]
        {
            new PermissionStereotype
            {
                Name = Administrator.ToString(),
                Permissions = new[] { 
                    ManageCsaInfo,
                    ManageBrkInfo,
                    ManageInfosaInfo,
                    ManageVivatChemijaInfo,
                    ManageIndiInfo,
                    ManageEsaInfo,
                    ManageFumsaInfo,
                    ManageStatiusInfo,
                    ManageVfsaInfo,
                    ManageShmInfo,
                },
            },
            new PermissionStereotype
            {
                Name = President.ToString(),
                Permissions = new[] { ManageCsaInfo },
            },
            new PermissionStereotype
            {
                Name = CsaEditor.ToString(),
                Permissions = new[] { ManageCsaInfo },
            },
            new PermissionStereotype
            {
                Name = BrkEditor.ToString(),
                Permissions = new[] { ManageBrkInfo },
            },
            new PermissionStereotype
            {
                Name = InfosaEditor.ToString(),
                Permissions = new[] { ManageInfosaInfo },
            },
            new PermissionStereotype
            {
                Name = VivatChemijaEditor.ToString(),
                Permissions = new[] { ManageVivatChemijaInfo },
            },
            new PermissionStereotype
            {
                Name = IndiEditor.ToString(),
                Permissions = new[] { ManageIndiInfo },
            },
            new PermissionStereotype
            {
                Name = EsaEditor.ToString(),
                Permissions = new[] { ManageEsaInfo },
            },
            new PermissionStereotype
            {
                Name = FumsaEditor.ToString(),
                Permissions = new[] { ManageFumsaInfo },
            },
            new PermissionStereotype
            {
                Name = StatiusEditor.ToString(),
                Permissions = new[] { ManageStatiusInfo },
            },
            new PermissionStereotype
            {
                Name = VfsaEditor.ToString(),
                Permissions = new[] { ManageVfsaInfo },
            },
            new PermissionStereotype
            {
                Name = ShmEditor.ToString(),
                Permissions = new[] { ManageShmInfo },
            },
        };
    }
}