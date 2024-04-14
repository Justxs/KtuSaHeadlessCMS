using HtmlAgilityPack;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaModule.Extensions;

public static class StringExtensions
{
    private static readonly char[] Separator = [' ', '\r', '\n'];

    public static bool IsLtLanguage(this string language)
    {
        return language.Equals(Languages.LT.ToString(), StringComparison.CurrentCultureIgnoreCase);
    }

    public static string CalculateReadingTime(this string htmlBody)
    {
        var textContent = HtmlTagRemoveRegex()
            .Replace(htmlBody, string.Empty);
        var totalWordCount = textContent.CountWords();

        var readingTimeMinutes = totalWordCount / 100;

        readingTimeMinutes = Math.Max(readingTimeMinutes, 1);

        return readingTimeMinutes > 1 ? $"{readingTimeMinutes} min." : "1 min.";
    }

    public static string GetPreviewText(this string htmlBody)
    {
        var textContent = HtmlTagRemoveRegex()
            .Replace(htmlBody, string.Empty);

        var words = textContent.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        var wordCount = Math.Min(50, words.Length);
        var first50Words = string.Join(" ", words, 0, wordCount);

        return first50Words;
    }

    private static int CountWords(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        var matches = WordCountRegex().Matches(input);

        return matches.Count;
    }

    public static List<string> GetContentList(this string htmlBody)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlBody);

        var h1NodeCollection = doc.DocumentNode.SelectNodes("//h1");

        var h1Tags = h1NodeCollection != null
            ? h1NodeCollection.Select(node => node.InnerText).ToList()
            : [];

        return h1Tags;
    }

    public static string AddH1Id(this string htmlBody)
    {

        return H1TagRegex().Replace(htmlBody, match =>
        {
            var innerText = match.Groups[1].Value;
            var id = innerText.GenerateIdFromInnerText();

            return $"<h1 id=\"{id}\">{innerText}</h1>";
        });
    }

    private static string GenerateIdFromInnerText(this string innerText)
    {
        var id = WhiteSpacesRegex().Replace(innerText, "-");
        id = LettersAndNumbersRegex().Replace(id, "");

        return id;
    }
}