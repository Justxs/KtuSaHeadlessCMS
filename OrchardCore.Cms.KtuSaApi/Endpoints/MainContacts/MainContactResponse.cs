namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class MainContactResponse
{
    [Description("Contact email address")] public required string Email { get; set; }

    [Description("Physical address")] public required string Address { get; set; }

    [Description("Contact phone number")] public required string PhoneNumber { get; set; }
}