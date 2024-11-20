namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class ContactDto
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ImageSrc { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Responsibilities { get; set; } = null!;

    public int Index { get; set; }
}