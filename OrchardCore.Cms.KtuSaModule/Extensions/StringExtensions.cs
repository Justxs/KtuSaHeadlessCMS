using HtmlAgilityPack;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaModule.Extensions;

public static class StringExtensions
{
    private static readonly char[] Separator = [' ', '\r', '\n'];

    extension(string language)
    {
        public bool IsLtLanguage()
        {
            return language.Equals(nameof(Languages.LT), StringComparison.CurrentCultureIgnoreCase);
        }

        public string CalculateReadingTime()
        {
            var textContent = HtmlTagRemoveRegex()
                .Replace(language, string.Empty);
            var totalWordCount = textContent.CountWords();

            var readingTimeMinutes = totalWordCount / 100;

            readingTimeMinutes = Math.Max(readingTimeMinutes, 1);

            return readingTimeMinutes > 1 ? $"{readingTimeMinutes} min." : "1 min.";
        }

        public string GetPreviewText()
        {
            var textContent = HtmlTagRemoveRegex()
                .Replace(language, string.Empty);

            var words = textContent.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            var wordCount = Math.Min(50, words.Length);
            var first50Words = string.Join(" ", words, 0, wordCount);

            return first50Words;
        }

        private int CountWords()
        {
            if (string.IsNullOrEmpty(language)) return 0;

            var matches = WordCountRegex().Matches(language);

            return matches.Count;
        }

        public List<string> GetContentList()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(language);

            var h1NodeCollection = doc.DocumentNode.SelectNodes("//h1");

            var h1Tags = h1NodeCollection.Select(node => node.InnerText).ToList();

            return h1Tags;
        }

        public string AddH1Id()
        {
            return H1TagRegex().Replace(language, match =>
            {
                var innerText = match.Groups[1].Value;
                var id = innerText.GenerateIdFromInnerText();

                return $"<h1 id=\"{id}\">{innerText}</h1>";
            });
        }

        private string GenerateIdFromInnerText()
        {
            var id = WhiteSpacesRegex().Replace(language, "-");
            id = LettersAndNumbersRegex().Replace(id, "");

            return id;
        }
    }
}