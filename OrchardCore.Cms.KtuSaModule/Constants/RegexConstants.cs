using System.Text.RegularExpressions;

namespace OrchardCore.Cms.KtuSaModule.Constants;

public static partial class RegexConstants
{
    [GeneratedRegex("<.*?>")]
    public static partial Regex HtmlTagRemoveRegex();

    [GeneratedRegex(@"\b\S+\b")]
    public static partial Regex WordCountRegex();

    [GeneratedRegex(@"<h[1-6][^>]*>(.*?)</h[1-6]>", RegexOptions.IgnoreCase)]
    public static partial Regex HtmlHeadingTagRegex();
}