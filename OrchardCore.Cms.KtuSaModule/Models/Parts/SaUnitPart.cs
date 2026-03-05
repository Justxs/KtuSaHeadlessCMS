using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class SaUnitPart : ContentPart
{
    public MediaField UnitPhoto { get; set; } = new();

    public string UnitName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? LinkedInUrl { get; set; }

    public string? FacebookUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;
}