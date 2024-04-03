using System.Text.Json.Serialization;

namespace OrchardCore.Cms.KtuSaModule.Models.ExternalApiResponse;

public class FientaEvent
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("starts_at")]
    public string StartsAt { get; set; } = null!;

    [JsonPropertyName("ends_at")]
    public string EndsAt { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = null!;
}