using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class EventPermissions : IPermissionProvider
{
    public static readonly Permission ManageCsaEvents = new(nameof(ManageCsaEvents), "Can manage CSA events.");
    public static readonly Permission ManageBrkEvents = new(nameof(ManageBrkEvents), "Can manage BRK events.");
    public static readonly Permission ManageInfosaEvents = new(nameof(ManageInfosaEvents), "Can manage InfoSA events.");
    public static readonly Permission ManageVivatChemijaEvents = new(nameof(ManageVivatChemijaEvents), "Can manage Vivat Chemija events.");
    public static readonly Permission ManageIndiEvents = new(nameof(ManageIndiEvents), "Can manage InDi events.");
    public static readonly Permission ManageEsaEvents = new(nameof(ManageEsaEvents), "Can manage ESA events.");
    public static readonly Permission ManageFumsaEvents = new(nameof(ManageFumsaEvents), "Can manage FUMSA events.");
    public static readonly Permission ManageStatiusEvents = new(nameof(ManageStatiusEvents), "Can manage Statius events.");
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
                Name = "Administrator",
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
                Name = "CsaEditor",
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
                Name = "BrkEditor",
                Permissions = new[] { ManageBrkEvents },
            },
            new PermissionStereotype
            {
                Name = "InfosaEditor",
                Permissions = new[] { ManageInfosaEvents },
            },
            new PermissionStereotype
            {
                Name = "VivatChemijaEditor",
                Permissions = new[] { ManageVivatChemijaEvents },
            },
            new PermissionStereotype
            {
                Name = "IndiEditor",
                Permissions = new[] { ManageIndiEvents },
            },
            new PermissionStereotype
            {
                Name = "EsaEditor",
                Permissions = new[] { ManageEsaEvents },
            },
            new PermissionStereotype
            {
                Name = "FumsaEditor",
                Permissions = new[] { ManageFumsaEvents },
            },
            new PermissionStereotype
            {
                Name = "StatiusEditor",
                Permissions = new[] { ManageStatiusEvents },
            },
            new PermissionStereotype
            {
                Name = "VfsaEditor",
                Permissions = new[] { ManageVfsaEvents },
            },
            new PermissionStereotype
            {
                Name = "ShmEditor",
                Permissions = new[] { ManageShmEvents },
            },
        };
    }
}