namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class ContactResponse
{
    [Description("Content item ID")] public string Id { get; set; } = null!;

    [Description("Full name of the member")]
    public string Name { get; set; } = null!;

    [Description("Email address of the member")]
    public string Email { get; set; } = null!;

    [Description("File ID of the member's profile photo")]
    public string ImageSrc { get; set; } = null!;

    [Description("Position title in the requested language")]
    public string Position { get; set; } = null!;

    [Description("Description of responsibilities in the requested language")]
    public string Responsibilities { get; set; } = null!;

    [Description("Display order index")] public int Index { get; set; }
}