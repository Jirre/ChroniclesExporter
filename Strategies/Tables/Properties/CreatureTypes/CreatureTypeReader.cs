using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.CreatureTypes;

public class CreatureTypeReader : MdReader<CreatureType>
{
    protected override bool TryGetProperties(string pLine, CreatureType pData)
    {
        return MarkdownUtility.TryParseLinkGuids(pLine,  "Parent:", e => pData.Parent = e[0]) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Features:", ELink.CreatureTypesFeatures, pData.Id) ||
               pLine.StartsWith("Children:");
    }
}
