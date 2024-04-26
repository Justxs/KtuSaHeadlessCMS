using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using YesSql;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class Repository(ISession session) : IRepository
{
    public async Task<IEnumerable<ContentItem>> GetAllAsync(string contentTypeName)
    {
        var contentItems = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == contentTypeName && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();

        return contentItems;
    }

    public async Task<ContentItem> GetSaUnitByName(SaUnit saUnit)
    {
        var saUnits = await GetAllAsync(ContentTypeConstants.SaUnit);

        return saUnits.First(unit => unit.As<SaUnitPart>().UnitName == saUnit.ToString());
    }
}