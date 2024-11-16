using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Skills)]
public class SkillSettings : ISettings<Skill, SkillReader, SkillWriter>
{
    public string FilePath => "Skills";

    public string Url(IRow pData)
    {
        return "/Skills?id={0}";
    }

    public string LinkClasses(IRow pData)
    {
        return "link-skill tooltip tooltip-skill";
    }

    public string LinkIcon(IRow pData)
    {
        return "skill";
    }

    public ETable[] Dependencies => [];
}
