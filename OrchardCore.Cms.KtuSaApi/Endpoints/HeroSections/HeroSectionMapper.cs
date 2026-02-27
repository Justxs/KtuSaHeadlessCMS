using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.HeroSections;

public static class HeroSectionMapper
{
    public static HeroSectionResponse ToResponse(this ContentItem item, bool isLithuanian)
    {
        var part = item.As<HeroSectionPart>();
        return new HeroSectionResponse
        {
            Title = isLithuanian ? part.TitleLt : part.TitleEn,
            Description = isLithuanian ? part.DescriptionLt.Text : part.DescriptionEn.Text,
            ImgSrc = part.ImageUploadField.FileId
        };
    }
}