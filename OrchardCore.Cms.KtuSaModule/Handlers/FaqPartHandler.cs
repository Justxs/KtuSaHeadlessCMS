using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class FaqPartHandler : DisplayTextPartHandler<FaqPart>
{
    protected override string GetDisplayText(FaqPart part) => $"{part.QuestionLt} / {part.QuestionEn}";
}
