namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class ContactResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("Full name of the member.")]
    public required string Name { get; set; }

    [Description("Email address of the member.")]
    public required string Email { get; set; }

    [Description("Public URL of the member profile photo.")]
    public required string ImageSrc { get; set; }

    [Description("Position title in the requested language. Can be empty when not linked.")]
    public required string Position { get; set; }

    [Description("Responsibilities text in the requested language. Can be empty when not linked.")]
    public required string Responsibilities { get; set; }

    [Description("Display order index (ascending).")]
    public int Index { get; set; }
}
