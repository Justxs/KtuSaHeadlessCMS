namespace OrchardCore.Cms.KtuSaModule.Dtos;

/// <summary>
/// Root response model returned by the Fienta public events API.
/// </summary>
public class FientaApiResponse
{
    /// <summary>
    /// Success metadata object returned by Fienta.
    /// </summary>
    public Dictionary<string, object> Success { get; set; } = null!;

    /// <summary>
    /// Timing metadata object returned by Fienta.
    /// </summary>
    public Dictionary<string, object> Time { get; set; } = null!;

    /// <summary>
    /// Total number of events included in the response.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Event list payload.
    /// </summary>
    public List<FientaEvent> Events { get; set; } = null!;
}