namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public class MainContactResponse
{
    [Description("Primary contact email address.")]
    public required string Email { get; set; }

    [Description("Primary physical address.")]
    public required string Address { get; set; }

    [Description("Primary contact phone number.")]
    public required string PhoneNumber { get; set; }
}