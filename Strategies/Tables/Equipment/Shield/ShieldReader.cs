using ChroniclesExporter.IO;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Shield;

public class ShieldReader : MdReader<Shield>
{
    protected override bool TryGetProperties(string pLine, ref Shield pData)
    {
        if (pLine.TryTrimStart("AC Bonus:", out string pAc) &&
            short.TryParse(pAc, System.Globalization.NumberStyles.Integer, 
                System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out short pIntAc))
        {
            pData.AcBonus = pIntAc;
            return true;
        }
        
        if (pLine.TryTrimStart("Max Dex Bonus:", out string pDexBonus) &&
            short.TryParse(pDexBonus, System.Globalization.NumberStyles.Integer, 
                System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out short pIntDexBonus))
        {
            pData.MaxDex = pIntDexBonus;
            return true;
        }

        if (pLine.TryTrimStart("Cost:", out string pCost) &&
            float.TryParse(pCost, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out float pFloatCost))
        {
            pData.Cost = pFloatCost;
            return true;
        }
        
        if (pLine.TryTrimStart("Weight:", out string pWeight) &&
            float.TryParse(pWeight, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out float pFloatWeight))
        {
            pData.Weight = pFloatWeight;
            return true;
        }
        
        if (pLine.TryTrimStart("Min Strength Score:", out string pStr) &&
            short.TryParse(pStr, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out short pIntStr))
        {
            pData.MinStr = pIntStr;
            return true;
        }

        if (pLine.TryTrimStart("Traits:", out string pTraits))
        {
            GetGuids(pTraits, ref pData);
            return true;
        }
        
        return false;
    }
    
    private static void GetGuids(string pLine, ref Shield pContainer)
    {
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length == 0)
            return;
        foreach (Guid trait in linkGuids)
            TableHandler.RegisterLink(ELink.ShieldTraits, new Link(pContainer.Id, trait));
    }
}
