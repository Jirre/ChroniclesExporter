using System.Text.RegularExpressions;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;

namespace ChroniclesExporter.Parse;

public static partial class ParserLocalLink
{
    
    [GeneratedRegex(@"[A-Za-z]+%20([a-fA-F0-9]{32})\.md")]
    private static partial Regex FileRegex();

    [LinkParseFunction(99)]
    private static bool GetLocalLink(string pHref, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        if (!pHref.TryMatch(FileRegex(), out Match localMatch)) return false;
        HtmlNode parent = pNode.ParentNode;
        
        HtmlNode link = pDoc.CreateElement("LocalLink");
        link.SetAttributeValue("target", localMatch.Groups[1].Value);

        if (TableHandler.TryGet(new Guid(localMatch.Groups[1].Value), out TableEntry entry) &&
            SettingsHandler.TryGetSettings(entry.Id, out ISettings<IRow> settings) &&
            entry.Row != null)
            link.SetAttributeValue("icon", settings.LinkIcon(entry.Row));

        link.InnerHtml = pNode.InnerHtml;
        parent.ReplaceChild(link, pNode);
        return true;
    }


}
