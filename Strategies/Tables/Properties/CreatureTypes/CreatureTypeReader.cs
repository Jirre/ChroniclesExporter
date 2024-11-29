using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.CreatureTypes;

public class CreatureTypeReader : MdReader<CreatureType>
{
    protected override bool TryGetProperties(string pLine, ref CreatureType pData)
    {
        if (pLine.TryTrimStart("Parent:", out string guids))
        {
            GetParent(guids, ref pData);
            return true;
        }

        return pLine.StartsWith("Children:");
    }
    
    private static void GetParent(string pLine, ref CreatureType pContainer)
    {
        Guid[] linkGuids = MarkdownUtility.GetLinkGuids(pLine);
        if (linkGuids.Length != 1)
            return;
        
        pContainer.Parent = linkGuids[0];
    }
}
