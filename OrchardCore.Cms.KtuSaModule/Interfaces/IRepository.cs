using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IRepository
{
    Task<IEnumerable<ContentItem>> GetAllAsync(string contentTypeName);

    Task<ContentItem> GetByIdAsync(string id);
}