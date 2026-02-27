using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Articles;

public class GetArticleByIdEndpoint(IContentManager contentManager)
    : Endpoint<GetArticleByIdRequest, ArticleContentResponse>
{
    public override void Configure()
    {
        Get("api/{language}/Articles/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Articles")
            .WithSummary("Get an article by ID")
            .WithDescription("Returns the full content of an article including HTML body, reading time estimate and a structured content list. Language: 'lt' or 'en'.")
            .Produces<ArticleContentResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetArticleByIdRequest req, CancellationToken ct)
    {
        var article = await contentManager.GetAsync(req.Id);

        if (article is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await contentManager.LoadAsync(article);

        var isLithuanian = req.Language.IsLtLanguage();

        var part = article.As<CardPart>();
        var htmlPart = article.As<ArticlePart>();

        var articleDto = new ArticleContentResponse
        {
            Title = (isLithuanian ? part?.TitleLt : part?.TitleEn)!,
            HtmlBody = (isLithuanian ? htmlPart?.HtmlContentLt.HtmlBody : htmlPart?.HtmlContentEn.HtmlBody)!,
            Id = article.ContentItemId,
            CreatedDate = (DateTime)article.CreatedUtc!,
            ThumbnailImageId = part!.ImageUploadField.FileId,
        };

        articleDto.ReadingTime = articleDto.HtmlBody.CalculateReadingTime();
        articleDto.ContentList = articleDto.HtmlBody.GetContentList();

        await Send.OkAsync(articleDto, ct);
    }
}
