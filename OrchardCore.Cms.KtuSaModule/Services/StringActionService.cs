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

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagRemoveRegex();

    [GeneratedRegex(@"\b\S+\b")]
    private static partial Regex WordCountRegex();
}