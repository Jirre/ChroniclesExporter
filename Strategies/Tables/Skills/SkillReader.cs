using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Traits;

public class SkillReader : MdReader<Skill>
{
    protected override bool TryGetProperties(string pLine, Skill pData)
    {
        return MarkdownUtility.TryParseEnum<EAbilities>(pLine, "Ability Score:", e => pData.Ability = e) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.SkillTraits, pData.Id);
    }
}
