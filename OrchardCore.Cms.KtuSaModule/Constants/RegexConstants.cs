using System.Text.RegularExpressions;

namespace OrchardCore.Cms.KtuSaModule.Constants;

public partial class RegexConstants
{
    [GeneratedRegex("<.*?>")]
    public static partial Regex HtmlTagRemoveRegex();

    [GeneratedRegex(@"\b\S+\b")]
    public static partial Regex WordCountRegex();

    [GeneratedRegex(@"<h1>(.*?)<\/h1>")]
    public static partial Regex H1TagRegex();

    [GeneratedRegex(@"\s+")]
    public static partial Regex WhiteSpacesRegex();

    [GeneratedRegex(@"[^0-9a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ\-]")]
    public static partial Regex LettersAndNumbersRegex();
}