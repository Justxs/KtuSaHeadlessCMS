using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class DocumentPartHandler : DisplayTextPartHandler<DocumentPart>
{
    protected override string GetDisplayText(DocumentPart part)
    {
        return $"{part.TitleLt} / {part.TitleEn}";
    }
}