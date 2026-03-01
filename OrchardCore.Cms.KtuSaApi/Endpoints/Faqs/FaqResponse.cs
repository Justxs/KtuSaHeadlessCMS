namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class FaqResponse
{
    [Description("Content item ID")] public required string Id { get; set; }

    [Description("FAQ question text in the requested language")]
    public required string Question { get; set; }

    [Description("FAQ answer text in the requested language")]
    public required string Answer { get; set; }

    [Description("Date and time the item was last modified (UTC)")]
    public DateTime ModifiedDate { get; set; }
}
