using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class CardPartHandler : DisplayTextPartHandler<CardPart>
{
    protected override string GetDisplayText(CardPart part) => $"{part.TitleLt} / {part.TitleEn}";
}