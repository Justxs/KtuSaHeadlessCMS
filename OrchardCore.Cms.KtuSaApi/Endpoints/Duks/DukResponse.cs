namespace OrchardCore.Cms.KtuSaApi.Endpoints.Duks;

public class DukResponse
{
    [Description("Content item ID")] public string Id { get; set; } = null!;

    [Description("FAQ question text in the requested language")]
    public string Question { get; set; } = null!;

    [Description("FAQ answer text in the requested language")]
    public string Answer { get; set; } = null!;

    [Description("Date and time the item was last modified (UTC)")]
    public DateTime ModifiedDate { get; set; }
}