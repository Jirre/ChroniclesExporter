using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace ChroniclesExporter.Utility;

public static partial class StringUtility
{
    [GeneratedRegex("[0-9a-fA-F]{32}")]
    private static partial Regex UrlGuidRegex();

    public static bool TryExtractGuidFromString(string pUrl, out Guid pGuid)
    {
        pGuid = Guid.Empty;
        if (string.IsNullOrEmpty(pUrl)) return false; // Handle empty or null URL

        // Use negative lookbehind to exclude "%20" before the 32 hex characters
        Regex regex = UrlGuidRegex();
        MatchCollection matches = regex.Matches(HttpUtility.UrlDecode(pUrl));

        // Check if there's a match and return the captured string
        if (matches.Count > 0)
            pGuid = new Guid(matches.Last().Value);

        return matches.Count > 0;
    }


    [GeneratedRegex(@"\[(?<text>.+?)\]\s*:\s*(?<url>\S+)(?:\s+""(?<title>.+?)"")?", RegexOptions.Compiled)]
    public static partial Regex MarkdownLinkRegex();

    public static string MarkdownLinkToHtml(string pMarkdown)
    {
        Regex regex = MarkdownLinkRegex();

        return regex.Replace(pMarkdown, pMatch =>
        {
            string text = pMatch.Groups["text"].Value;
            string url = pMatch.Groups["url"].Value;
            string title = pMatch.Groups["title"].Captures.Count > 0 ? pMatch.Groups["title"].Value : text;

            return $"<a href=\"{url}\">{title}</a>";
        });
    }

    public static string TrimStart(this string pValue, string pSubstring)
    {
        return pValue.StartsWith(pSubstring) ? pValue.Remove(0, pSubstring.Length).TrimStart(' ') : pValue;
    }

    public static string TrimStart(this string pValue, string pSubstring, bool pIgnoreCase, CultureInfo? pCultureInfo)
    {
        return pValue.StartsWith(pSubstring, pIgnoreCase, pCultureInfo)
            ? pValue.Remove(0, pSubstring.Length).TrimStart(' ')
            : pValue;
    }

    public static bool TryTrimStart(this string pValue, string pSubstring, out string? pResult)
    {
        if (pValue.StartsWith(pSubstring, true, CultureInfo.InvariantCulture))
        {
            pResult = pValue.Remove(0, pSubstring.Length).Trim();
            return true;
        }

        pResult = "";
        return false;
    }
    
    public static bool TryTrimStart(this string pValue, string pSubstring, out string pResult, bool pIgnoreCase,
        CultureInfo? pCultureInfo)
    {
        if (pValue.StartsWith(pSubstring, pIgnoreCase, pCultureInfo))
        {
            pResult = pValue.Remove(0, pSubstring.Length).Trim();
            return true;
        }

        pResult = "";
        return false;
    }
    
    public static bool TryMatch(this string pValue, Regex pRegex, out Match pMatch)
    {
        pMatch = pRegex.Match(pValue);
        return pMatch.Success;
    }
}
