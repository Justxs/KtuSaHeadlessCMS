using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class PositionPartHandler : DisplayTextPartHandler<PositionPart>
{
    protected override string GetDisplayText(PositionPart part)
    {
        return $"{part.NameLt} / {part.NameEn}";
    }
}