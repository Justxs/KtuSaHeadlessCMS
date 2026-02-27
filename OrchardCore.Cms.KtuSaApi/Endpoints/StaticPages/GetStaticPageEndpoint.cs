using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class GetStaticPageEndpoint(IRepository repository)
    : Endpoint<GetStaticPageRequest, StaticPageResponse>
{
    public override void Configure()
    {
        Get("api/{language}/StaticPages/{pageName}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Static Pages")
            .WithSummary("Get a static page by name")
            .WithDescription(
                "Returns the HTML body of a static page whose display text contains the given pageName. Language: 'lt' or 'en'.")
            .Produces<StaticPageResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetStaticPageRequest req, CancellationToken ct)
    {
        var staticPages = await repository.GetAllAsync(StaticPage);
        var isLithuanian = req.Language.IsLtLanguage();

        var page = staticPages.FirstOrDefault(p => p.DisplayText.Contains(req.PageName));

        if (page is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(page.ToResponse(isLithuanian), ct);
    }
}