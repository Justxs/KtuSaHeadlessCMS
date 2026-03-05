using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class GetStaticPageEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetStaticPageRequest, StaticPageResponse>
{
    public override void Configure()
    {
        Get("api/static-pages/{pageName}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Static Pages")
            .WithSummary("Get a static page by name")
            .WithDescription(
                "Returns the first published static page whose localized title contains the provided pageName (case-insensitive match). " +
                "Use query parameter language=en (default) or language=lt.")
            .Produces<StaticPageResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetStaticPageRequest req, CancellationToken ct)
    {
        var staticPages = await repository.GetAllAsync(StaticPage);
        var language = req.Language;

        var page = staticPages.FirstOrDefault(p =>
            language.Resolve(p.As<StaticPagePart>()?.TitleLt, p.As<StaticPagePart>()?.TitleEn)!
                .Contains(req.PageName, StringComparison.CurrentCultureIgnoreCase));

        if (page is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(page.ToResponse(language, mediaFileStore), ct);
    }
}