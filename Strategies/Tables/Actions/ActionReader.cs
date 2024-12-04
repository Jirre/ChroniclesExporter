using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Actions;

public class ActionReader : MdReader<Action>
{
    protected override bool TryGetProperties(string pLine, Action pData)
    {
        return MarkdownUtility.TryParseString(pLine, "Details:", e => pData.Details = e) ||
               MarkdownUtility.TryParseEnum<EActionTypes>(pLine, "Type:", e => pData.Type = e);
    }
}
