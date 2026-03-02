using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public static class FaqMapper
{
    extension(ContentItem item)
    {
        public FaqResponse ToFaqResponse(Language language)
        {
            var part = item.As<FaqPart>();
            return new FaqResponse
            {
                Id = item.ContentItemId,
                Question = language.Resolve(part?.QuestionLt, part?.QuestionEn)!,
                Answer = language.Resolve(part?.AnswerLt, part?.AnswerEn)!,
                ModifiedDate = item.ModifiedUtc ?? DateTime.MinValue
            };
        }
    }
}
