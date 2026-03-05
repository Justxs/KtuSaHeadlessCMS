using OrchardCore.Cms.KtuSaModule.Dtos;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IFientaService
{
    /// <summary>
    /// Fetches organizer events from Fienta for the specified locale.
    /// </summary>
    /// <param name="locale">Locale code expected by Fienta (for example, "lt" or "en").</param>
    /// <returns>List of events returned by Fienta.</returns>
    Task<List<FientaEvent>> FetchKtuSaEventsAsync(string locale);
}