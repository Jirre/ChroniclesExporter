using ChroniclesExporter.IO;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Languages;

public class LanguageReader : MdReader<Language>
{
    protected override bool TryGetProperties(string pLine, ref Language pData)
    {
        if (pLine.TryTrimStart("Rarity:", out string pRarity) &&
            Enum.TryParse(pRarity, true, out ERarities pRarityEnum))
        {
            pData.Rarity = pRarityEnum;
            return true;
        }

        if (pLine.TryTrimStart("Scripts:", out string pScripts) &&
            MarkdownUtility.TryGetLinkGuids(pLine, out Guid[] pGuids))
        {
            pData.Script = pGuids[0];
            return true;
        }

        if (pLine.TryTrimStart("Traits:", out string pTraits))
        {
            GetGuids(pTraits, ref pData);
            return true;
        }
        
        return false;
    }
    
    private static void GetGuids(string pLine, ref Language pContainer)
    {
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length == 0)
            return;
        foreach (Guid trait in linkGuids)
            TableHandler.RegisterLink(ELink.LanguageTraits, new Link(pContainer.Id, trait));
    }
}
