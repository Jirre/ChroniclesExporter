using ChroniclesExporter.IO;
using ChroniclesExporter.Strategy.Links;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Traits;

public class SkillReader : MdReader<Skill>
{
    protected override bool TryGetProperties(string pLine, ref Skill pData)
    {
        if (pLine.StartsWith("Ability Score:"))
        {
            pLine = pLine.TrimStart("Ability Score:").Trim();
            if (Enum.TryParse(pLine, true, out EAbilities ability))
            {
                pData.Ability = ability;
                return true;
            }
        }

        if (pLine.StartsWith("Traits:"))
        {
            GetGuids(pLine, ref pData);
            return true;
        }

        return false;
    }

    private static void GetGuids(string pLine, ref Skill pContainer)
    {
        pLine = pLine.TrimStart("Traits:");
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length == 0)
            return;
        foreach (Guid trait in linkGuids)
            TableHandler.RegisterLink(ELink.SkillTraits, new Link(pContainer.Id, trait));
    }
}
