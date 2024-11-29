using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Utility;

public static class MarkdownUtility
{
    public static Guid[] GetLinkGuids(string pLine, char pSeparator = ',')
    {
        if (string.IsNullOrWhiteSpace(pLine))
            return [];

        string[] urls = pLine.Split(pSeparator);
        if (urls.Length == 0)
            return [];

        List<Guid> guidList = new();
        foreach (string url in urls)
        {
            StringUtility.TryExtractGuidFromString(url, out Guid guid);
            if (TableHandler.Contains(guid))
                guidList.Add(guid);
        }

        return guidList.ToArray();
    }

    public static bool TryGetLinkGuids(string pLine, out Guid[] pGuids, char pSeparator = ',')
    {
        pGuids = GetLinkGuids(pLine);
        return pGuids.Length > 0;
    }

    public static bool TryRegisterLinks(string pLine, string pKey, ELink pLinkTable, Guid pSourceId)
    {
        if (!pLine.TryTrimStart(pKey, out string pResult) ||
            !TryGetLinkGuids(pResult, out Guid[] pGuidS)) return false;
        
        foreach (Guid guid in pGuidS)
        {
            TableHandler.RegisterLink(pLinkTable, new Link(pSourceId, guid));
        }

        return true;
    }

    public static bool TryGetEnumArray<T>(string pLine, string pKey, out T[] pResult, char pSeparator = ',') 
        where T : struct, Enum
    {
        pResult = [];
        if (!pLine.TryTrimStart(pKey, out string pSizes)) return false;
        
        string[] strings = pSizes.Trim().Split(',');
        List<T> result = [];
        foreach (string s in strings)
        {
            if (Enum.TryParse(s.Trim(), true, out T sizeEnum))
                result.Add(sizeEnum);
        }
        pResult = result.ToArray();
        return result.Count > 0;
    }
}
