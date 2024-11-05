using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Conditions;

[Settings(ETable.Conditions)]
public class ConditionSettings : ISettings<Condition, ConditionReader, ConditionWriter>
{
    public string FilePath => "Conditions";
    public string Url(Condition pData) => "/Conditions?id={0}";
    public string LinkClasses(Condition pData) => "link-condition tooltip tooltip-condition";
    public string LinkIcon(Condition pData) => "condition";

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
