using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Skills)]
public class SkillSettings : ISettings<Skill, SkillReader, SkillWriter>
{
    public string FilePath => "Skills";

    public string Url(Skill pData)
    {
        return "/Skills?id={0}";
    }

    public string LinkClasses(Skill pData)
    {
        return "link-skill tooltip tooltip-skill";
    }

    public string LinkIcon(Skill pData)
    {
        return "skill";
    }

    public ETable[] Dependencies => [];
}
