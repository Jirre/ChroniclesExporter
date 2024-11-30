using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Languages;

public class LanguageReader : MdReader<Language>
{
    protected override bool TryGetProperties(string pLine, Language pData)
    {
        return MarkdownUtility.TryParseEnum<ERarities>(pLine, "Rarity:", e => pData.Rarity = e) ||
               MarkdownUtility.TryParseLinkGuids(pLine, "Scripts:", e => pData.Script = e[0]) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.LanguageTraits, pData.Id);
    }
}
