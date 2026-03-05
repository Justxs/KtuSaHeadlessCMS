using OrchardCore.Cms.KtuSaApi.Extensions;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Faqs;

public static class FaqMapper
{
    extension(ContentItem item)
    {
        public FaqResponse ToFaqResponse(Language language, IMediaFileStore mediaFileStore)
        {
            var part = item.As<FaqPart>();
            return new FaqResponse
            {
                Id = item.ContentItemId,
                Question = language.Resolve(part?.QuestionLt, part?.QuestionEn)!,
                Answer = item.ToContentBlocks(language.FaqAnswerFlowPartName, mediaFileStore),
                ModifiedDate = item.ModifiedUtc ?? DateTime.MinValue
            };
        }
    }
}