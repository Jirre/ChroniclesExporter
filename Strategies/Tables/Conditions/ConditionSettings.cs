using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Conditions;

[Settings(ETable.Conditions)]
public class ConditionSettings : ISettings<Condition, ConditionReader, ConditionWriter>
{
    public string FilePath => "Conditions";

    public string Url(Condition pData)
    {
        return "/Conditions?id={0}";
    }

    public string LinkClasses(Condition pData)
    {
        return "link-condition tooltip tooltip-condition";
    }

    public string LinkIcon(Condition pData)
    {
        return "condition";
    }

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
