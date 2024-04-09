using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Dtos.Articles;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Interfaces;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ArticlesController(IContentManager contentManager, IRepository repository, IStringActionService stringActionService) : ControllerBase
{
    private static readonly string ArticleContentType = ContentTypeNames.Article.ToString();

    [HttpGet]
    [ProducesResponseType(typeof(List<ArticlePreviewDto>), 200)]
    public async Task<ActionResult> GetArticles(string language, [FromQuery] int? limit)
    {
        var articles = await repository.GetAllAsync(ArticleContentType);

        articles = articles.OrderByDescending(item => item.CreatedUtc);

        if (limit is not null)
        {
            articles = articles.Take((int)limit).ToList();
        }

        var isLithuanian = stringActionService.IsLanguageLithuanian(language);

        var articleDtos = articles.Select(item =>
        {
            var part = item.As<CardPart>();
            var dto = new ArticlePreviewDto
            {
                Id = item.ContentItemId,

                Title = (isLithuanian 
                    ? part?.TitleLt
                    : part?.TitleEn)!,

                Preview = (isLithuanian 
                    ? part?.PreviewLt 
                    : part?.PreviewEn)!,

                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = part!.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(articleDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(List<ArticleContentDto>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
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

        var articleDto = new ArticleContentDto
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