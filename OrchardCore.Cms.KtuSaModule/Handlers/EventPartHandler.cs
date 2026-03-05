using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class EventPartHandler : DisplayTextPartHandler<EventPart>
{
    protected override string GetDisplayText(EventPart part)
    {
        return $"{part.TitleLt} / {part.TitleEn}";
    }
}