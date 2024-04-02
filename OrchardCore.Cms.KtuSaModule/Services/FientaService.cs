using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.ExternalApiResponse;
using System.Net.Http;
using System.Text.Json;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class FientaService(HttpClient httpClient) : IFientaService
{
    // TODO Move to app settings BaseUrl and KtuSaId
    private const string BaseUrl = "https://fienta.com/api/v1/public/events";

    private const string KtuSaId = "84867";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<List<FientaEvent>> FetchKtuSaEvents(string locale = "en")
    {
        var requestUrl = $"{BaseUrl}?organizer={KtuSaId}&locale={locale}";

        var response = await httpClient.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch events from Fienta.");
        }

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        var eventsElement = root.GetProperty("events");

        var events = JsonSerializer.Deserialize<List<FientaEvent>>(
            eventsElement.GetRawText(),
            JsonSerializerOptions);

        return events ?? [];
    }
}