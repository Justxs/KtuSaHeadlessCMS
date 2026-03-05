using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class CategoryPartHandler : DisplayTextPartHandler<CategoryPart>
{
    protected override string GetDisplayText(CategoryPart part)
    {
        return $"{part.TitleLt} / {part.TitleEn}";
    }
}