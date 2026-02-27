using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.HeroSections;

public class GetHeroSectionEndpoint(IRepository repository)
    : Endpoint<GetHeroSectionRequest, HeroSectionResponse>
{
    public override void Configure()
    {
        Get("api/hero-sections/{sectionName}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Hero Sections")
            .WithSummary("Get a hero section by name")
            .WithDescription(
                "Returns the title, description and image of a hero section whose title contains the given sectionName (case-insensitive). " +
                "Pass language=lt or language=en.")
            .Produces<HeroSectionResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetHeroSectionRequest req, CancellationToken ct)
    {
        var heroSections = await repository.GetAllAsync(HeroSection);
        var isLithuanian = req.Language.IsLtLanguage();

        var section = heroSections
            .FirstOrDefault(item =>
                ((isLithuanian ? item.As<HeroSectionPart>()?.TitleLt : item.As<HeroSectionPart>()?.TitleEn)!)
                .Contains(req.SectionName, StringComparison.CurrentCultureIgnoreCase));

        if (section is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(section.ToResponse(isLithuanian), ct);
    }
}