using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.ExternalApiResponse;
using System.Text.Json;
using OrchardCore.Cms.KtuSaModule.Settings;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class FientaService(HttpClient httpClient, FientaSettings settings) : IFientaService
{
    private readonly string _baseUrl = settings.BaseUrl;

    private readonly string _organiserId = settings.OrganiserId;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<List<FientaEvent>> FetchKtuSaEvents(string locale = "en")
    {
        var requestUrl = $"{_baseUrl}?organizer={_organiserId}&locale={locale}";

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