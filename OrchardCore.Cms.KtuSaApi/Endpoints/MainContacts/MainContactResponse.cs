namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class MainContactResponse
{
    [Description("Contact email address")] public string Email { get; set; } = null!;

    [Description("Physical address")] public string Address { get; set; } = null!;

    [Description("Contact phone number")] public string PhoneNumber { get; set; } = null!;
}