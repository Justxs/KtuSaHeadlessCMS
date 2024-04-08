using OrchardCore.Security.Permissions;
using static OrchardCore.Cms.KtuSaModule.Models.Enums.Roles;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ContactPermissions : IPermissionProvider
{
    public static readonly Permission ManageCsaContacts = new(nameof(ManageCsaContacts), "Can manage CSA contacts.");
    public static readonly Permission ManageBrkContacts = new(nameof(ManageBrkContacts), "Can manage BRK contacts.");
    public static readonly Permission ManageInfosaContacts = new(nameof(ManageInfosaContacts), "Can manage InfoSA contacts.");
    public static readonly Permission ManageVivatChemijaContacts = new(nameof(ManageVivatChemijaContacts), "Can manage Vivat chemija contacts.");
    public static readonly Permission ManageIndiContacts = new(nameof(ManageIndiContacts), "Can manage InDi contacts.");
    public static readonly Permission ManageEsaContacts = new(nameof(ManageEsaContacts), "Can manage ESA contacts.");
    public static readonly Permission ManageFumsaContacts = new(nameof(ManageFumsaContacts), "Can manage FUMSA contacts.");
    public static readonly Permission ManageStatiusContacts = new(nameof(ManageStatiusContacts), "Can manage STATIUS contacts.");
    public static readonly Permission ManageVfsaContacts = new(nameof(ManageVfsaContacts), "Can manage VFSA contacts.");
    public static readonly Permission ManageShmContacts = new(nameof(ManageShmContacts), "Can manage SHM contacts.");

    public Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return Task.FromResult(new[]
            {
                ManageCsaContacts,
                ManageBrkContacts,
                ManageInfosaContacts,
                ManageVivatChemijaContacts,
                ManageIndiContacts,
                ManageEsaContacts,
                ManageFumsaContacts,
                ManageStatiusContacts,
                ManageVfsaContacts,
                ManageShmContacts,
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
                Permissions = new[]
                {
                    ManageCsaContacts,
                    ManageBrkContacts,
                    ManageInfosaContacts,
                    ManageVivatChemijaContacts,
                    ManageIndiContacts,
                    ManageEsaContacts,
                    ManageFumsaContacts,
                    ManageStatiusContacts,
                    ManageVfsaContacts,
                    ManageShmContacts,
                },
            },
            new PermissionStereotype
            {
                Name = CsaEditor.ToString(),
                Permissions = new[] { 
                    ManageCsaContacts,
                    ManageBrkContacts,
                    ManageInfosaContacts,
                    ManageVivatChemijaContacts,
                    ManageIndiContacts,
                    ManageEsaContacts,
                    ManageFumsaContacts,
                    ManageStatiusContacts,
                    ManageVfsaContacts,
                    ManageShmContacts,
                },
            },
            new PermissionStereotype
            {
                Name = BrkEditor.ToString(),
                Permissions = new[] { ManageBrkContacts },
            },
            new PermissionStereotype
            {
                Name = InfosaEditor.ToString(),
                Permissions = new[] { ManageInfosaContacts },
            },
            new PermissionStereotype
            {
                Name = VivatChemijaEditor.ToString(),
                Permissions = new[] { ManageVivatChemijaContacts },
            },
            new PermissionStereotype
            {
                Name = IndiEditor.ToString(),
                Permissions = new[] { ManageIndiContacts },
            },
            new PermissionStereotype
            {
                Name = EsaEditor.ToString(),
                Permissions = new[] { ManageEsaContacts },
            },
            new PermissionStereotype
            {
                Name = FumsaEditor.ToString(),
                Permissions = new[] { ManageFumsaContacts },
            },
            new PermissionStereotype
            {
                Name = StatiusEditor.ToString(),
                Permissions = new[] { ManageStatiusContacts },
            },
            new PermissionStereotype
            {
                Name = VfsaEditor.ToString(),
                Permissions = new[] { ManageVfsaContacts },
            },
            new PermissionStereotype
            {
                Name = ShmEditor.ToString(),
                Permissions = new[] { ManageShmContacts },
            }
        };
    }
}