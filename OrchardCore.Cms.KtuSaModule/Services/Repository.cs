using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore.ContentManagement.Records;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class Repository(IContentManager contentManager, ISession session) : IRepository
{
    public async Task<IEnumerable<ContentItem>> GetAllAsync(string contentTypeName)
    {
        var contentItems = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == contentTypeName && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();

        return contentItems;
    }

    public Task<ContentItem> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}