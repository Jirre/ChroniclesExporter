using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Skill_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.SkillTraits;

    protected override string TableName => "skills_traits";
    protected override string[] Fields => new[] {"skill_id, trait_id"};
}
