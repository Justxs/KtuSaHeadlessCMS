using HtmlAgilityPack;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaApi.Extensions;

public static class StringExtensions
{
    private static readonly char[] Separator = [' ', '\r', '\n'];

    extension(string text)
    {
        public string CalculateReadingTime()
        {
            var textContent = HtmlTagRemoveRegex()
                .Replace(text, string.Empty);
            var totalWordCount = textContent.CountWords();

            var readingTimeMinutes = totalWordCount / 200;

            readingTimeMinutes = Math.Max(readingTimeMinutes, 1);

            return readingTimeMinutes > 1 ? $"{readingTimeMinutes} min." : "1 min.";
        }

        public string GetPreviewText()
        {
            var textContent = HtmlTagRemoveRegex()
                .Replace(text, string.Empty);

            var words = textContent.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            var wordCount = Math.Min(50, words.Length);
            var first50Words = string.Join(" ", words, 0, wordCount);

            return first50Words;
        }

        private int CountWords()
        {
            if (string.IsNullOrEmpty(text)) return 0;

            var matches = WordCountRegex().Matches(text);

            return matches.Count;
        }

        public List<string> GetContentList()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            var headingNodes = doc.DocumentNode.SelectNodes("//h1 | //h2");
            if (headingNodes is null)
            {
                return [];
            }

            return headingNodes.Select(node => node.InnerText).ToList();
        }
    }
}
