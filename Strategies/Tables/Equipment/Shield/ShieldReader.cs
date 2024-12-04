using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Shield;

public class ShieldReader : MdReader<Shield>
{
    protected override bool TryGetProperties(string pLine, Shield pData)
    {
        return MarkdownUtility.TryParseNumber<short>(pLine, "AC Bonus:", e => pData.AcBonus = e) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Max Dex Bonus:", e => pData.MaxDex = e) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Cost:", e => pData.Cost = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Weight:", e => pData.Weight = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Min Strength Score:", e => pData.MinStr = e) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.ShieldTraits, pData.Id);
    }
}
