using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Conditions;

[Settings(ETable.Conditions)]
public class ConditionSettings : ISettings<Condition, ConditionReader, ConditionWriter>
{
    public string FilePath => "Conditions";

    public string Url(IRow pData)
    {
        return "/Conditions?id={0}";
    }

    public string LinkClasses(IRow pData)
    {
        return "link-condition tooltip tooltip-condition";
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
