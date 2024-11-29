using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitReader : MdReader<Trait>
{
    protected override bool TryGetProperties(string pLine, ref Trait pData)
    {
        if (MarkdownUtility.TryGetEnumArray(pLine, "Categories:", out ETraitCategories[] categories))
        {
            pData.Categories = categories;
            return true;
        }
        
        if (pLine.TryTrimStart("Index:", out string pIndex) &&
            int.TryParse(pIndex, out int pIntIndex))
        {
            pData.Priority = pIntIndex;
            return true;
        }

        return false;
    }
}
