using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public static class FaqMapper
{
    extension(ContentItem item)
    {
        public FaqResponse ToFaqResponse(Language language)
        {
            var part = item.As<FaqPart>();
            var flow = item.Get<FlowPart>(language.FaqAnswerFlowPartName);
            var answerHtml = string.Empty;

            if (flow?.Widgets is not null and { Count: > 0 })
            {
                answerHtml = string.Join("",
                    flow.Widgets
                        .Where(w => w.ContentType == ParagraphWidget)
                        .Select(w => w.As<ParagraphWidgetPart>()?.Body?.Html)
                        .Where(html => !string.IsNullOrEmpty(html)));
            }

            return new FaqResponse
            {
                Id = item.ContentItemId,
                Question = language.Resolve(part?.QuestionLt, part?.QuestionEn)!,
                Answer = answerHtml,
                ModifiedDate = item.ModifiedUtc ?? DateTime.MinValue
            };
        }
    }
}
