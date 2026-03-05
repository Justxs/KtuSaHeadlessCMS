using System.Text.Json.Serialization;

namespace OrchardCore.Cms.KtuSaModule.Dtos;

/// <summary>
/// Event payload returned by the Fienta public events API.
/// </summary>
public class FientaEvent
{
    /// <summary>
    /// Fienta event identifier.
    /// </summary>
    [JsonPropertyName("id")] public int Id { get; set; }

    /// <summary>
    /// Localized event title.
    /// </summary>
    [JsonPropertyName("title")] public string Title { get; set; } = null!;

    /// <summary>
    /// Event start date-time string returned by Fienta.
    /// </summary>
    [JsonPropertyName("starts_at")] public string StartsAt { get; set; } = null!;

    /// <summary>
    /// Event end date-time string returned by Fienta.
    /// </summary>
    [JsonPropertyName("ends_at")] public string EndsAt { get; set; } = null!;

    /// <summary>
    /// Event description HTML/text.
    /// </summary>
    [JsonPropertyName("description")] public string Description { get; set; } = null!;

    /// <summary>
    /// Event location/address.
    /// </summary>
    [JsonPropertyName("address")] public string Address { get; set; } = null!;

    /// <summary>
    /// Public Fienta event page URL.
    /// </summary>
    [JsonPropertyName("url")] public string Url { get; set; } = null!;

    /// <summary>
    /// Public event cover image URL.
    /// </summary>
    [JsonPropertyName("image_url")] public string ImageUrl { get; set; } = null!;
}
