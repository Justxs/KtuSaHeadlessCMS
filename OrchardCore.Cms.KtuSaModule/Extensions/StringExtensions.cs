using OrchardCore.Cms.KtuSaModule.Models.Enums;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaModule.Extensions;

public static class StringExtensions
{
    extension(string language)
    {
        public bool IsLtLanguage()
        {
            return language.Equals(nameof(Languages.LT), StringComparison.CurrentCultureIgnoreCase);
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