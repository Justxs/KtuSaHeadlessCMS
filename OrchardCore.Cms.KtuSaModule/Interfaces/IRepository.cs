using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IRepository
{
    Task<IEnumerable<ContentItem>> GetAllAsync(string contentTypeName);

    Task<ContentItem?> GetByIdAsync(string contentItemId);

    Task<IEnumerable<ContentItem>> GetByIdsAsync(IEnumerable<string> contentItemIds);

    Task<ContentItem?> GetSaUnitByNameAsync(SaUnit saUnit);
}