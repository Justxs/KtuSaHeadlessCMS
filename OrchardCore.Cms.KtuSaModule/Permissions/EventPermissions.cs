using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Models.Enums.Roles;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class EventPermissions : IPermissionProvider
{
    public static readonly Permission ManageCsaEvents = new(nameof(ManageCsaEvents), "Can manage CSA events.");
    public static readonly Permission ManageBrkEvents = new(nameof(ManageBrkEvents), "Can manage BRK events.");
    public static readonly Permission ManageInfosaEvents = new(nameof(ManageInfosaEvents), "Can manage InfoSA events.");
    public static readonly Permission ManageVivatChemijaEvents = new(nameof(ManageVivatChemijaEvents), "Can manage Vivat chemija events.");
    public static readonly Permission ManageIndiEvents = new(nameof(ManageIndiEvents), "Can manage InDi events.");
    public static readonly Permission ManageEsaEvents = new(nameof(ManageEsaEvents), "Can manage ESA events.");
    public static readonly Permission ManageFumsaEvents = new(nameof(ManageFumsaEvents), "Can manage FUMSA events.");
    public static readonly Permission ManageStatiusEvents = new(nameof(ManageStatiusEvents), "Can manage STATIUS events.");
    public static readonly Permission ManageVfsaEvents = new(nameof(ManageVfsaEvents), "Can manage VFSA events.");
    public static readonly Permission ManageShmEvents = new(nameof(ManageShmEvents), "Can manage SHM events.");


    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
        {
            ManageCsaEvents,
            ManageBrkEvents,
            ManageInfosaEvents,
            ManageVivatChemijaEvents,
            ManageIndiEvents,
            ManageEsaEvents,
            ManageFumsaEvents,
            ManageStatiusEvents,
            ManageVfsaEvents,
            ManageShmEvents,
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
                Permissions = new[] { 
                    ManageCsaEvents,
                    ManageBrkEvents,
                    ManageInfosaEvents,
                    ManageVivatChemijaEvents,
                    ManageIndiEvents,
                    ManageEsaEvents,
                    ManageFumsaEvents,
                    ManageStatiusEvents,
                    ManageVfsaEvents,
                    ManageShmEvents,
                },
            },
            new PermissionStereotype
            {
                Name = CsaEditor.ToString(),
                Permissions = new[]
                {
                    ManageCsaEvents,
                    ManageBrkEvents,
                    ManageInfosaEvents,
                    ManageVivatChemijaEvents,
                    ManageIndiEvents,
                    ManageEsaEvents,
                    ManageFumsaEvents,
                    ManageStatiusEvents,
                    ManageVfsaEvents,
                    ManageShmEvents,
                },
            },
            new PermissionStereotype
            {
                Name = BrkEditor.ToString(),
                Permissions = new[] { ManageBrkEvents },
            },
            new PermissionStereotype
            {
                Name = InfosaEditor.ToString(),
                Permissions = new[] { ManageInfosaEvents },
            },
            new PermissionStereotype
            {
                Name = VivatChemijaEditor.ToString(),
                Permissions = new[] { ManageVivatChemijaEvents },
            },
            new PermissionStereotype
            {
                Name = IndiEditor.ToString(),
                Permissions = new[] { ManageIndiEvents },
            },
            new PermissionStereotype
            {
                Name = EsaEditor.ToString(),
                Permissions = new[] { ManageEsaEvents },
            },
            new PermissionStereotype
            {
                Name = FumsaEditor.ToString(),
                Permissions = new[] { ManageFumsaEvents },
            },
            new PermissionStereotype
            {
                Name = StatiusEditor.ToString(),
                Permissions = new[] { ManageStatiusEvents },
            },
            new PermissionStereotype
            {
                Name = VfsaEditor.ToString(),
                Permissions = new[] { ManageVfsaEvents },
            },
            new PermissionStereotype
            {
                Name = ShmEditor.ToString(),
                Permissions = new[] { ManageShmEvents },
            },
        };
    }
}