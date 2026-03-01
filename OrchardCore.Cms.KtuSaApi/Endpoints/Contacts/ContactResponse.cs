namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class ContactResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("Full name of the member")]
    public required string Name { get; set; }

    [Description("Email address of the member")]
    public required string Email { get; set; }

    [Description("File ID of the member's profile photo")]
    public required string ImageSrc { get; set; }

    [Description("Position title in the requested language")]
    public required string Position { get; set; }

    [Description("Description of responsibilities in the requested language")]
    public required string Responsibilities { get; set; }

    [Description("Display order index")] public int Index { get; set; }
}