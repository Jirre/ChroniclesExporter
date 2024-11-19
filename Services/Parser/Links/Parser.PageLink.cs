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
    // ReSharper disable once UnusedMember.Local
    private static bool GetPageLink(string pHref, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        if (!pHref.TryMatch(UrlRegex(), out Match urlMatch)) return false;
        HtmlNode parent = pNode.ParentNode;
        Guid guid = new Guid(urlMatch.Groups[1].Value);
        HtmlNode link = pDoc.CreateElement("PageLink");
        if (TableHandler.TryGet(guid, out TableEntry entry) &&
            SettingsHandler.TryGetSettings(entry.Id, out ISettings settings) &&
            entry.Row != null)
        {
            link.SetAttributeValue("target",
                string.IsNullOrWhiteSpace(settings.Url(entry.Row))
                    ? guid.ToString()
                    : string.Format(settings.Url(entry.Row), guid.ToString()));
            link.SetAttributeValue("icon", settings.LinkIcon(entry.Row));
            string iconClasses = settings.LinkIconClasses(entry.Row);
            if (!string.IsNullOrWhiteSpace(iconClasses))
                link.SetAttributeValue("iconClasses", iconClasses);
        }
        else
        {
            link.SetAttributeValue("target", guid.ToString());
        }

        link.InnerHtml = pNode.InnerHtml;
        parent.ReplaceChild(link, pNode);
        return true;
    }
}
