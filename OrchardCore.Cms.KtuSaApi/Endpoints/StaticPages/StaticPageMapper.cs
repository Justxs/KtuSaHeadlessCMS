using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public static class StaticPageMapper
{
    public static StaticPageResponse ToResponse(this ContentItem item, bool isLithuanian)
    {
        var part = item.As<StaticPagePart>();
        return new StaticPageResponse
        {
            Body = isLithuanian ? part.BodyLt.HtmlBody : part.BodyEn.HtmlBody
        };
    }
}