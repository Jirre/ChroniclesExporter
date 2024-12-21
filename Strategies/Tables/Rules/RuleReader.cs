using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Rules;

public class RuleReader : MdReader<Rule>
{
    protected override bool TryGetProperties(string pLine, Rule pData)
    {
        if (MarkdownUtility.TryParseNumber<short>(pLine, "Priority:", e => pData.Priority = e) ||
            MarkdownUtility.TryParseEnum<ERuleCategory>(pLine, "Categories:", e => pData.Category = e))
            return true;
        
        if (string.IsNullOrWhiteSpace(pData.Content))
            pData.Description = pLine;
        
        return false;
    }
}
