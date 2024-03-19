using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Permissions;

public class ContactPermissions : IPermissionProvider
{
    public static readonly Permission ManageCsaContacts = new(nameof(ManageCsaContacts), "Can manage CSA contacts.");
    public static readonly Permission ManageBrkContacts = new(nameof(ManageBrkContacts), "Can manage BRK contacts.");
    public static readonly Permission ManageInfosaContacts = new(nameof(ManageInfosaContacts), "Can manage InfoSA contacts.");
    public static readonly Permission ManageVivatChemijaContacts = new(nameof(ManageVivatChemijaContacts), "Can manage Vivat Chemija contacts.");
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
                Name = "Administrator",
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
        };
    }
}