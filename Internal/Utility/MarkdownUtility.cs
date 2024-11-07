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
}
