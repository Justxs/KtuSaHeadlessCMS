using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Dtos;
using System.Text.RegularExpressions;
using OrchardCore.Cms.KtuSaModule.Models.Enums;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class ArticlesController(IContentManager contentManager, ISession session) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetArticles(string language)
    {
        var articles = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == "Article" && index.Published)
            .OrderByDescending(index => index.CreatedUtc)
            .ListAsync();

        foreach (var article in articles)
        {
            await contentManager.LoadAsync(article);
        }

        var isLithuanian = language.ToUpper() == Languages.LT.ToString();

        var articleDtos = articles.Select(item =>
        {
            var part = item.As<ArticlePart>();
            var dto = new ArticleDto
            {
                Title = (isLithuanian 
                    ? part?.TitleLt
                    : part?.TitleEn)!,

                Preview = (isLithuanian 
                    ? part?.PreviewLt 
                    : part?.PreviewEn)!,

                HtmlBody = (isLithuanian 
                    ? part?.HtmlContentLt.Html 
                    : part?.HtmlContentEn.Html)!,

                CreatedDate = (DateTime)item.CreatedUtc!,
                ThumbnailImageId = part!.ImageUploadField.FileId,
            };

            dto.ReadingTime = CalculateReadingTime(dto.Preview , dto.HtmlBody);

            return dto;
        }).ToList();

        return Ok(articleDtos);
    }

    private static string CalculateReadingTime(string preview, string htmlBody)
    {
        var textContent = Regex.Replace(htmlBody, "<.*?>", string.Empty);
        var totalWordCount = CountWords(preview) + CountWords(textContent);

        var readingTimeMinutes = totalWordCount / 100;

        readingTimeMinutes = Math.Max(readingTimeMinutes, 1);

        return readingTimeMinutes > 1 ? $"{readingTimeMinutes} min." : "1 min.";
    }

    private static int CountWords(string input)
    {
        if (string.IsNullOrEmpty(input)) return 0;

        var matches = Regex.Matches(input, @"\b\S+\b");

        return matches.Count;
    }
}