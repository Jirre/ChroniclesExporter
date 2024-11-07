using ChroniclesExporter.IO;
using ChroniclesExporter.Strategy.Links;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Conditions;

public class ConditionReader : MdReader<Condition>
{
    protected override bool TryGetProperties(string pLine, ref Condition pData)
    {
        if (pLine.StartsWith("Traits:"))
        {
            GetGuids(pLine, ref pData);
            return true;
        }

        return false;
    }

    private static void GetGuids(string pLine, ref Condition pContainer)
    {
        pLine = pLine.TrimStart("Traits:");
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length == 0)
            return;
        foreach (Guid trait in linkGuids)
            TableHandler.RegisterLink(ELink.ConditionTraits, new Link(pContainer.Id, trait));
    }
}
