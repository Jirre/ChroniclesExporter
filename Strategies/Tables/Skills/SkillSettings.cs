using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Skills)]
public class SkillSettings : ISettings<SkillReader, SkillWriter>
{
    public string FilePath => "Skills";
    public string Url => "/Skills?id={0}";
    public string LinkClasses => "link-skill tooltip tooltip-skill";
    public string LinkIcon => "skill";
    public ETable[] Dependencies => [];
}
