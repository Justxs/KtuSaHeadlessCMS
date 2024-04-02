using OrchardCore.Cms.KtuSaModule.Models.ExternalApiResponse;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IFientaService
{
    Task<List<FientaEvent>> FetchKtuSaEvents(string locale);
}