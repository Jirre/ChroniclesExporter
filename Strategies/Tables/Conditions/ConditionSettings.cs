using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Conditions;

[Settings(ETable.Conditions)]
public class ConditionSettings : ISettings<ConditionReader, ConditionWriter>
{
    public string FilePath => "Conditions";

    public string Url(IRow pData)
    {
        return "/conditions?id={0}";
    }

    public string LinkIcon(IRow pData)
    {
        return "condition";
    }
    
    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
