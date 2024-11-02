using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Actions;

[Settings(ETable.Actions)]
public class ActionSettings : ISettings<ActionReader, ActionWriter>
{
    public string FilePath => "Actions";
    public string Url => "/Actions?id={0}";
    public string LinkClasses => "link-condition tooltip tooltip-condition";
    public string LinkIcon => "action";
    public ETable[] Dependencies => [];
}
