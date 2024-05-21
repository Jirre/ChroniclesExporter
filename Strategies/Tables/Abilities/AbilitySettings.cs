using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Abilities;

[Settings(ETable.Abilities)]
public class AbilitySettings : ISettings<AbilityReader, AbilityWriter>
{
    public string FilePath => "Abilities";
    public string Url => "/Abilities?id={0}";
    public string LinkClasses => "link-ability tooltip tooltip-ability";
    public ETable[] Dependencies => Array.Empty<ETable>();
}
