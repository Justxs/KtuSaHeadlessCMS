using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Duks;

public static class DukMapper
{
    extension(ContentItem item)
    {
        public DukResponse ToResponse(bool isLithuanian)
        {
            var part = item.As<DukPart>();
            return new DukResponse
            {
                Id = item.ContentItemId,
                Question = (isLithuanian ? part?.QuestionLt : part?.QuestionEn)!,
                Answer = (isLithuanian ? part?.AnswerLt : part?.AnswerEn)!,
                ModifiedDate = (DateTime)item.ModifiedUtc!
            };
        }
    }
}