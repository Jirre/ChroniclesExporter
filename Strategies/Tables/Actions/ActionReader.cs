using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Actions;

public class ActionReader : MdReader<Action>
{
    protected override bool TryGetProperties(string pLine, ref Action pData)
    {
        if (TryGetDetails(pLine, ref pData)) return true;
        if (pLine.StartsWith("Type:"))
        {
            pLine = pLine.TrimStart("Type:").Trim();
            if (Enum.TryParse(pLine, true, out EActionTypes type))
            {
                pData.Type = type;
                return true;
            }
        }

        return false;
    }

    private static bool TryGetDetails(string pLine, ref Action pData)
    {
        if (!pLine.StartsWith("Details:") ||
            !string.IsNullOrWhiteSpace(pData.Details))
            return false;

        pData.Details = pLine.TrimStart("Details: ");
        return true;
    }
}
