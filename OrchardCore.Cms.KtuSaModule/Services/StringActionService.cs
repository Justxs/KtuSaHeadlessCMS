using HtmlAgilityPack;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using System.Text.RegularExpressions;

namespace OrchardCore.Cms.KtuSaModule.Services;

public partial class StringActionService : IStringActionService
{
    public bool IsLanguageLithuanian(string language)
    {
        return language.Equals(Languages.LT.ToString(), StringComparison.CurrentCultureIgnoreCase);
    }

    public string CalculateReadingTime(string preview, string htmlBody)
    {
        var textContent = HtmlTagRemoveRegex().Replace(htmlBody, string.Empty);
        var totalWordCount = CountWords(preview) + CountWords(textContent);

        var readingTimeMinutes = totalWordCount / 100;

        readingTimeMinutes = Math.Max(readingTimeMinutes, 1);

        return readingTimeMinutes > 1 ? $"{readingTimeMinutes} min." : "1 min.";
    }

    private static int CountWords(string input)
    {
        if (string.IsNullOrEmpty(input)) return 0;

        var matches = WordCountRegex().Matches(input);

        return matches.Count;
    }

    public List<string> GetContentList(string htmlBody)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlBody);

        var h1Tags = doc.DocumentNode.SelectNodes("//h1").Select(node => node.InnerText).ToList();

        return h1Tags;
    }

    public string AddH1Id(string htmlBody)
    {

        return H1TagRegex().Replace(htmlBody, match =>
        {
            var innerText = match.Groups[1].Value;
            var id = GenerateIdFromInnerText(innerText);

            return $"<h1 id=\"{id}\">{innerText}</h1>";
        });
    }

    private static string GenerateIdFromInnerText(string innerText)
    {
        var id = WhiteSpacesRegex().Replace(innerText, "-");
        id = LettersAndNumbersRegex().Replace(id, "");

        return id;
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagRemoveRegex();

    [GeneratedRegex(@"\b\S+\b")]
    private static partial Regex WordCountRegex();

    [GeneratedRegex(@"<h1>(.*?)<\/h1>")]
    private static partial Regex H1TagRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpacesRegex();

    [GeneratedRegex(@"[^0-9a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ\-]")]
    private static partial Regex LettersAndNumbersRegex();
}