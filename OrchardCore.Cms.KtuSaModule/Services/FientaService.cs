using OrchardCore.Cms.KtuSaModule.Interfaces;
using System.Text.Json;
using OrchardCore.Cms.KtuSaModule.Dtos.ExternalApiResponse;
using OrchardCore.Cms.KtuSaModule.Settings;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class FientaService(HttpClient httpClient, FientaSettings settings) : IFientaService
{
    private readonly string _baseUrl = settings.BaseUrl;

    private readonly string _organiserId = settings.OrganiserId;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<FientaEvent>> FetchKtuSaEventsAsync(string locale)
    {
        var requestUrl = $"{_baseUrl}?organizer={_organiserId}&locale={locale}";

        var response = await httpClient.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode) return [];

        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<FientaApiResponse>(json, JsonSerializerOptions);

        return apiResponse?.Events ?? [];
    }
}