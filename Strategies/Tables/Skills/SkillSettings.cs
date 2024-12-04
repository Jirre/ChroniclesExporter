using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Skills)]
public class SkillSettings : ISettings<SkillReader, SkillWriter>
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

    public string LinkIconClasses(IRow pData)
    {
        if (pData is not Skill skill)
            return "";

        return skill.Ability switch
        {
            EAbilities.Strength => "strength",
            EAbilities.Dexterity => "dexterity",
            EAbilities.Intelligence => "intelligence",
            EAbilities.Wisdom => "wisdom",
            EAbilities.Charisma => "charisma",
            EAbilities.Constitution => "constitution",
            _ => throw new ArgumentOutOfRangeException($"Unknown Abilities: {skill.Ability}")
        };
    }

    public ETable[] Dependencies => [];
}
