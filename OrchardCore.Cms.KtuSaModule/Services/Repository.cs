using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class Repository(ISession session, IContentManager contentManager) : IRepository
{
    public async Task<IEnumerable<ContentItem>> GetAllAsync(string contentTypeName)
    {
        return await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == contentTypeName && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();
    }

    public async Task<ContentItem?> GetByIdAsync(string contentItemId)
    {
        return await contentManager.GetAsync(contentItemId);
    }

    public async Task<IEnumerable<ContentItem>> GetByIdsAsync(IEnumerable<string> contentItemIds)
    {
        return await contentManager.GetAsync(contentItemIds);
    }

    public async Task<ContentItem?> GetSaUnitByNameAsync(SaUnit saUnit)
    {
        var saUnits = await GetAllAsync(ContentTypeConstants.SaUnit);

        return saUnits.FirstOrDefault(unit => unit.As<SaUnitPart>().UnitName == saUnit.ToString());
    }
}