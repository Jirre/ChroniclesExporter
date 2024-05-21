using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Conditions;

[Settings(ETable.Conditions)]
public class ConditionSettings : ISettings<ConditionReader, ConditionWriter>
{
    public string FilePath => "Conditions";
    public string Url => "/Conditions?id={0}";
    public string LinkClasses => "link-condition tooltip tooltip-condition";
    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
