using System.Text.RegularExpressions;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;

namespace ChroniclesExporter.Parse;

public static partial class ParserPageLink
{
    [GeneratedRegex(@".*\/[A-Za-z\-]+-([a-fA-F0-9]{32})(\?|$)")]
    private static partial Regex UrlRegex();
    
    [LinkParseFunction(100)]
    private static bool GetPageLink(string pHref, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        if (!pHref.TryMatch(UrlRegex(), out Match urlMatch)) return false;
        HtmlNode parent = pNode.ParentNode;
        
        HtmlNode link = pDoc.CreateElement("PageLink");
        if (TableHandler.TryGet(new Guid(urlMatch.Groups[1].Value), out TableEntry entry) &&
            SettingsHandler.TryGetSettings(entry.Id, out ISettings<IRow> settings) &&
            entry.Row != null)
        {
            link.SetAttributeValue("target",
                string.IsNullOrWhiteSpace(settings.Url(entry.Row))
                    ? urlMatch.Groups[1].Value
                    : string.Format(settings.Url(entry.Row), urlMatch.Groups[1].Value));
            link.SetAttributeValue("icon", settings.LinkIcon(entry.Row));
        }
        else
        {
            link.SetAttributeValue("target", urlMatch.Groups[1].Value);
        }

        link.InnerHtml = pNode.InnerHtml;
        parent.ReplaceChild(link, pNode);
        return true;
    }
}
