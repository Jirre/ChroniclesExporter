using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Races;

public class RaceReader : MdReader<Race>
{
    protected override bool TryGetProperties(string pLine, Race pData)
    {
        return MarkdownUtility.TryParseEnum<ERarities>(pLine, "Rarity:", e => pData.Rarity = e) ||
               MarkdownUtility.TryParseEnum<EAbilities>(pLine, "Ability:", e => pData.Ability = e) ||
               MarkdownUtility.TryParseEnumArray<ESizes>(pLine, "Sizes:", e => pData.Sizes = e) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Speed:", e => pData.Speed = e) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Creature Types:", ELink.RaceCreatureTypes, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Features:", ELink.RaceFeatures, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Languages:", ELink.RaceLanguages, pData.Id) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.RaceTraits, pData.Id);
    }
}
