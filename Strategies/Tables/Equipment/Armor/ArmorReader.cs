using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Armor;

public class ArmorReader : MdReader<Armor>
{
    protected override bool TryGetProperties(string pLine, Armor pData)
    {
        return MarkdownUtility.TryParseEnum<EArmorCategories>(pLine, "Category:", e => pData.Category = e) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "AC:", e => pData.Ac = e) ||
               MarkdownUtility.TryParseString(pLine, "Dex Bonus:", e => pData.AddDex = e == "Yes") ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Max Dex Bonus:", e => pData.MaxDex = e) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Cost:", e => pData.Cost = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Weight:", e => pData.Weight = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Min Strength Score:", e => pData.MinStr = e) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Speed Penalty:", e => pData.SpeedPenalty = e) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.ArmorTraits, pData.Id);
    }
}
