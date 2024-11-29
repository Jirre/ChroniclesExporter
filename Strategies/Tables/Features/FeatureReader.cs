using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Features;

public class FeatureReader : MdReader<Feature>
{
    protected override bool TryGetProperties(string pLine, ref Feature pData)
    {
        if (pLine.TryTrimStart("Index:", out string pStringIndex) &&
            short.TryParse(pStringIndex, out short pResult))
        {
            pData.Index = pResult;
            return true;
        }
        
        return false;
    }
}
