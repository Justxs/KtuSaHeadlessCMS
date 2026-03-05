using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public class FaqResponse
{
    [Description("Unique content item ID.")]
    public required string Id { get; set; }

    [Description("FAQ question text in the requested language.")]
    public required string Question { get; set; }

    [Description("Structured content blocks of the FAQ answer in the requested language.")]
    public required List<ContentBlockResponse> Answer { get; set; }

    [Description("Last modification timestamp in UTC.")]
    public DateTime ModifiedDate { get; set; }
}