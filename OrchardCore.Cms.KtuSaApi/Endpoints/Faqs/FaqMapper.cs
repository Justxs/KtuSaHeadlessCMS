using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public static class FaqMapper
{
    extension(ContentItem item)
    {
        public FaqResponse ToFaqResponse(bool isLithuanian)
        {
            var part = item.As<FaqPart>();
            return new FaqResponse
            {
                Id = item.ContentItemId,
                Question = (isLithuanian ? part?.QuestionLt : part?.QuestionEn)!,
                Answer = (isLithuanian ? part?.AnswerLt : part?.AnswerEn)!,
                ModifiedDate = (DateTime)item.ModifiedUtc!
            };
        }
    }
}
