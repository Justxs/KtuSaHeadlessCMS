using System.Text.Json.Serialization;

namespace OrchardCore.Cms.KtuSaModule.Models.ExternalApiResponse;

public class FientaEvent
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("starts_at")]
    public DateTime StartsAt { get; set; }

    [JsonPropertyName("ends_at")]
    public DateTime EndsAt { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = null!;
}