using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaModule.Models.Parts;

public class EventPart : ContentPart
{
    public string TitleLt { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string FbEventLink { get; set; } = null!;

    public string? Address { get; set; }

    public string? FientaTicketLinkLt { get; set; }

    public string? FientaTicketLinkEn { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public MediaField CoverImage { get; set; } = new();

    public ContentPickerField OrganisersField { get; set; } = null!;
}