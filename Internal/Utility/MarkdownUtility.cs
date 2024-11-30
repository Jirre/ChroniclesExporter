using System.Numerics;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Utility;

public static class MarkdownUtility
{
    public static bool TryParseLinkGuids(string pLine, string pKey, Action<Guid[]> pOutput, char pSeparator = ',')
    {
        if (string.IsNullOrWhiteSpace(pLine) || !pLine.TryTrimStart(pKey, out string guidString))
            return false;

        string[] urls = guidString.Split(pSeparator);
        if (urls.Length == 0)
            return false;

        List<Guid> guidList = new();
        foreach (string url in urls)
        {
            StringUtility.TryExtractGuidFromString(url, out Guid guid);
            if (TableHandler.Contains(guid))
                guidList.Add(guid);
        }

        pOutput(guidList.ToArray());
        return true;
    }

    public static bool TryRegisterLinks(string pLine, string pKey, ELink pLinkTable, Guid pSourceId)
    {
        return TryParseLinkGuids(pLine, pKey, pGuids =>
                {
                    foreach (Guid guid in pGuids)
                    {
                        TableHandler.RegisterLink(pLinkTable, new Link(pSourceId, guid));
                    }
                });
    }

    public static bool TryParseString(string pLine, string pKey, Action<String> pOutput, char pSeparator = ',')
    {
        if (!pLine.TryTrimStart(pLine, out string pResult)) 
            return false;
        
        pOutput(pResult);
        return true;
    }
    
    public static bool TryParseNumber<T>(string pLine, string pKey, Action<T> pOutput, 
        System.Globalization.NumberStyles pStyles = System.Globalization.NumberStyles.Integer, 
        System.Globalization.NumberFormatInfo? pFormat = null)
        where T : INumberBase<T>
    {
        if (!pLine.TryTrimStart(pKey, out string pStringValue) ||
            !T.TryParse(pStringValue, pStyles,
                pFormat ?? System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out T? result)) return false;
        
        pOutput(result);
        return true;
    }

    public static bool TryParseEnum<T>(string pLine, string pKey, Action<T> pOutput)
        where T : struct, Enum
    {
        if (!pLine.TryTrimStart(pKey, out string pEnumString) ||
            !Enum.TryParse(pEnumString.Trim(), true, out T result)) return false;
        
        pOutput(result);
        return true;
    }
    
    public static bool TryParseEnumArray<T>(string pLine, string pKey, Action<T[]> pOutput, char pSeparator = ',') 
        where T : struct, Enum
    {
        if (!pLine.TryTrimStart(pKey, out string pSizes)) return false;
        
        string[] strings = pSizes.Trim().Split(',');
        List<T> result = [];
        foreach (string s in strings)
        {
            if (Enum.TryParse(s.Trim(), true, out T sizeEnum))
                result.Add(sizeEnum);
        }
        pOutput(result.ToArray());
        return true;
    }
}
