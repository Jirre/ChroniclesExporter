using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Features;

public class FeatureReader : MdReader<Feature>
{
    protected override bool TryGetProperties(string pLine, Feature pData)
    {
        return MarkdownUtility.TryParseNumber<short>(pLine, "Index:", e => pData.Index = e) ||
               MarkdownUtility.TryParseEnumArray<EActionTypes>(pLine, "Action Types:", e => pData.Actions = e);
    }
}
