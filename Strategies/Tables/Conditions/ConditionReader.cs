using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Strategy.Conditions;

public class ConditionReader : MdReader<Condition>
{
    protected override bool TryGetProperties(string pLine, Condition pData)
    {
        return MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.ConditionTraits, pData.Id);
    }
}
