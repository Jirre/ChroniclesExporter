using ChroniclesExporter.IO;
using ChroniclesExporter.Strategy.Links;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitReader : MdReader<Trait>
{
    protected override bool TryGetProperties(string pLine, ref Trait pData)
    {
        if (pLine.StartsWith("Categories:"))
        {
            pLine = pLine.TrimStart("Categories:").Trim();
            string[] traits = pLine.Split(',');
            ETraitCategories[] traitCategories = traits.Select(Enum.Parse<ETraitCategories>).ToArray();
            pData.Categories = traitCategories;
            return true;
        }

        if (pLine.StartsWith("Index:"))
        {
            GetPriority(pLine, ref pData);
            return true;
        }

        return false;
    }

    private static void GetGuids(string pLine, ref Trait pContainer)
    {
        pLine = pLine.TrimStart("Categories:");
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length == 0)
            return;
        foreach (Guid category in linkGuids)
            TableHandler.RegisterLink(ELink.TraitCategories, new Link(pContainer.Id, category));
    }

    private static void GetPriority(string pLine, ref Trait pContainer)
    {
        if (int.TryParse(pLine.TrimStart("Index:"), out int result))
            pContainer.Priority = result;
        pContainer.Priority = 0;
    }
}
