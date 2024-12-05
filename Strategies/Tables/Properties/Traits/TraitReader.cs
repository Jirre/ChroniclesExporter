using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitReader : MdReader<Trait>
{
    protected override bool TryGetProperties(string pLine, Trait pData)
    {
        return MarkdownUtility.TryParseEnumArray<ETraitCategories>(pLine, "Categories:", e => pData.Categories = e) ||
               MarkdownUtility.TryParseString(pLine, "Icon:", e => pData.Icon = e) ||
               MarkdownUtility.TryParseString(pLine, "Class:", e => pData.Class = e) ||
               MarkdownUtility.TryParseNumber<int>(pLine, "Priority:", e => pData.Priority = e);
    }
}
