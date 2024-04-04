using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

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

    public QuillField BodyFieldLt { get; set; } = null!;

    public QuillField BodyFieldEn { get; set; } = null!;

    public ImageUploadField ImageUploadField { get; set; } = null!;

    public ContentPickerField OrganisersField { get; set; } = null!;
}