﻿using System.Globalization;
using System.Text.RegularExpressions;

namespace ChroniclesExporter.Utility;

public static partial class StringUtility
{
    [GeneratedRegex(@"(?<!%20)[0-9a-fA-F]{32}")]
    private static partial Regex UrlGuidRegex();
    public static bool TryExtractGuidFromString(string pUrl, out Guid pGuid)
    {
        pGuid = Guid.Empty;
        if (string.IsNullOrEmpty(pUrl))
        {
            return false; // Handle empty or null URL
        }

        // Use negative lookbehind to exclude "%20" before the 32 hex characters
        Regex regex = UrlGuidRegex();
        MatchCollection matches = regex.Matches(pUrl);

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
        
        return regex.Replace(pMarkdown, match =>
        {
            string text = match.Groups["text"].Value;
            string url = match.Groups["url"].Value;
            string title = match.Groups["title"].Captures.Count > 0 ? match.Groups["title"].Value : text;

            return $"<a href=\"{url}\">{title}</a>";
        });
    }

    public static string TrimStart(this string pValue, string pSubstring)
    {
        return pValue.StartsWith(pSubstring) ? 
            pValue.Remove(0, pSubstring.Length).TrimStart(' ') : 
            pValue;
    }
    
    public static string TrimStart(this string pValue, string pSubstring, bool pIgnoreCase, CultureInfo? pCultureInfo)
    {
        return pValue.StartsWith(pSubstring, pIgnoreCase, pCultureInfo) ? 
            pValue.Remove(0, pSubstring.Length).TrimStart(' ') : 
            pValue;
    }
}