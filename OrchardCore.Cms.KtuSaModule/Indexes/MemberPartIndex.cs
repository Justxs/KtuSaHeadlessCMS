using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace OrchardCore.Cms.KtuSaModule.Indexes;

public class MemberPartIndex : MapIndex
{
    public string ContentItemId { get; set; } = null!;

    public SaUnit SaUnit { get; set; }
}

public class MemberPartIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<MemberPartIndex>().Map(contentItem =>
        {
            var memberPart = contentItem.As<MemberPart>();
            return (memberPart is null 
                ? null 
                : new MemberPartIndex
                {
                    ContentItemId = contentItem.ContentItemId,
                    SaUnit = memberPart.SaUnit,
                })!;
        });
    }
}