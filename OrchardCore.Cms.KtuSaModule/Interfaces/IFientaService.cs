using OrchardCore.Cms.KtuSaModule.Dtos;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IFientaService
{
    Task<List<FientaEvent>> FetchKtuSaEventsAsync(string locale);
}