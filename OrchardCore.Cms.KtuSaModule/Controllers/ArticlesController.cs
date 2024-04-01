using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Services;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ArticlesController(IContentManager contentManager, ISession session, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string ArticleContentType = ContentTypeNames.Article.ToString();

    [HttpGet]
    public async Task<ActionResult> GetArticles(string language, [FromQuery] int? limit)
    {
        var articles = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == ArticleContentType && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();

        if (limit is not null)
        {
            articles = articles.Take((int)limit).ToList();
        }

        foreach (var article in articles)
        {
            await contentManager.LoadAsync(article);
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var articleDtos = articles.Select(item =>
        {
            var part = item.As<CardPart>();
            var dto = new ArticleDto
            {
                Title = (isLithuanian 
                    ? part?.TitleLt
                    : part?.TitleEn)!,

                Preview = (isLithuanian 
                    ? part?.PreviewLt 
                    : part?.PreviewEn)!,

                Id = item.ContentItemId,
                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = part!.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(articleDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetArticleById(string language, string id)
    {
        var article = await contentManager.GetAsync(id);

        if (article is null)
        {
            return NotFound("Article not found");
        }

        if (!article.Published)
        {
            return BadRequest("Article is not published yet.");
        }

        await contentManager.LoadAsync(article);

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var part = article.As<CardPart>();
        var htmlPart = article.As<ArticlePart>();

        var articleDto = new ArticleDto
        {
            Title = (isLithuanian
                ? part?.TitleLt
                : part?.TitleEn)!,

            Preview = (isLithuanian
                ? part?.PreviewLt
                : part?.PreviewEn)!,

            HtmlBody = (isLithuanian
                ? htmlPart?.HtmlContentLt.HtmlBody
                : htmlPart?.HtmlContentEn.HtmlBody)!,


            Id = article.ContentItemId,
            CreatedDate = (DateTime)article.CreatedUtc!,
            ThumbnailImageId = part!.ImageUploadField.FileId,
        };

        articleDto.ReadingTime = stringActionService.CalculateReadingTime(articleDto.Preview, articleDto.HtmlBody);
        articleDto.ContentList = stringActionService.GetContentList(articleDto.HtmlBody);

        return Ok(articleDto);
    }
}