namespace OrchardCore.Cms.KtuSaApi.Endpoints.Duks;

public class GetDuksRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;

    [Description("Optional. When provided, returns a random subset of that many items")]
    public int? Limit { get; set; }
}