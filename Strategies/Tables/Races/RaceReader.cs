using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Races;

public class RaceReader : MdReader<Race>
{
    protected override bool TryGetProperties(string pLine, ref Race pData)
    {
        if (pLine.TryTrimStart("Rarity:", out string pRarity) &&
            Enum.TryParse(pRarity, true, out ERarities pRarityEnum))
        {
            pData.Rarity = pRarityEnum;
            return true;
        }
        
        if (pLine.TryTrimStart("Ability:", out string pAbility) &&
            Enum.TryParse(pAbility, true, out EAbilities pAbilityEnum))
        {
            pData.Ability = pAbilityEnum;
            return true;
        }

        if (MarkdownUtility.TryGetEnumArray(pLine, "Sizes:", out ESizes[] pResult))
        {
            pData.Sizes = pResult;
            return true;
        }
        
        if (pLine.TryTrimStart("Speed:", out string pSpeed) &&
            short.TryParse(pSpeed, out short pSpeedInt))
        {
            pData.Speed = pSpeedInt;
            return true;
        }

        return MarkdownUtility.TryRegisterLinks(pLine, "Creature Types:", ELink.RaceCreatureTypes, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Features:", ELink.RaceFeatures, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Languages:", ELink.RaceLanguages, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.RaceTraits, pData.Id);
    }
}
