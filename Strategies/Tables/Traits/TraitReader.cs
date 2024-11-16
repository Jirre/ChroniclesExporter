using ChroniclesExporter.IO;
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
            foreach (string trait in traits)
            {
                if (Enum.TryParse(trait.Trim(), true, out ETraitCategories category))
                    TableHandler.RegisterLink(ELink.TraitCategories, new CategoryLink(pData.Id, category));
            }
            return true;
        }
        
        if (pLine.StartsWith("Index:"))
        {
            GetPriority(pLine, ref pData);
            return true;
        }

        return false;
    }

    private static void GetPriority(string pLine, ref Trait pContainer)
    {
        if (int.TryParse(pLine.TrimStart("Index:"), out int result))
            pContainer.Priority = result;
        pContainer.Priority = 0;
    }
}
