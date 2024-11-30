using System.Numerics;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Utility;

public static class MarkdownUtility
{
    /// <summary>
    /// Attempts to parse Notion Style Relationship Links to usable Guids
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pOutput">Setter which is called if the data is found</param>
    /// <param name="pSeparator">Separator of each guid/link</param>
    /// <returns>Whether the Parse was successful</returns>
    public static bool TryParseLinkGuids(string pLine, string pKey, Action<Guid[]> pOutput, char pSeparator = ',')
    {
        if (string.IsNullOrWhiteSpace(pLine) || !pLine.TryTrimStart(pKey, out string guidString))
            return false;

        string[] urls = guidString.Split(pSeparator);
        if (urls.Length == 0)
            return true;

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

    /// <summary>
    /// Attempts to register Notion Style Relationship Links to a Link Table
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pLinkTable">What table to write these links to</param>
    /// <param name="pSourceId">Guid of the source of the link</param>
    /// <returns>Whether the registration was successful</returns>
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

    /// <summary>
    /// Attempts to parse a field string to a data setter
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pOutput">Setter which is called if the data is found</param>
    /// <returns>Whether the Parse was successful</returns>
    public static bool TryParseString(string pLine, string pKey, Action<string> pOutput)
    {
        if (!pLine.TryTrimStart(pKey, out string pResult))
            return false;

        pOutput(pResult);
        return true;
    }
    
    /// <summary>
    /// Attempts to parse a field number to a data setter
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pOutput">Setter which is called if the data is found</param>
    /// <param name="pStyles">NumberStyle to use (defaults to Integer)</param>
    /// <param name="pFormat">NumberFormatInfo to use (defaults to InvariantCulture.NumberFormat)</param>
    /// <typeparam name="T">Type to parse inherited from INumberBase</typeparam>
    /// <returns>Whether the Parse was successful</returns>
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

    /// <summary>
    /// Attempts to parse a field enum to a data setter
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pOutput">Setter which is called if the data is found</param>
    /// <typeparam name="T">Type to parse inherited from Enum</typeparam>
    /// <returns>Whether the Parse was successful</returns>
    public static bool TryParseEnum<T>(string pLine, string pKey, Action<T> pOutput)
        where T : struct, Enum
    {
        if (!pLine.TryTrimStart(pKey, out string pEnumString) ||
            !Enum.TryParse(pEnumString.Trim(), true, out T result)) return false;

        pOutput(result);
        return true;
    }

    /// <summary>
    /// Attempts to parse a field of multiple enums to a data setter
    /// </summary>
    /// <param name="pLine">String Line of Markdown Text</param>
    /// <param name="pKey">Field Name used as identifier</param>
    /// <param name="pOutput">Setter which is called if the data is found</param>
    /// <param name="pSeparator">Separator of each entry</param>
    /// <typeparam name="T">Type to parse inherited from Enum</typeparam>
    /// <returns>Whether the Parse was successful</returns>
    public static bool TryParseEnumArray<T>(string pLine, string pKey, Action<T[]> pOutput, char pSeparator = ',')
        where T : struct, Enum
    {
        if (!pLine.TryTrimStart(pKey, out string pSizes)) return false;

        string[] strings = pSizes.Trim().Split(pSeparator);
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
