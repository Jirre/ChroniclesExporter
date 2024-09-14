using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Skill_Traits : MySqlLinkWriter<Link>
{
    public override ELink LinkId => ELink.SkillTraits;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command =
            new MySqlCommand(
                "INSERT INTO chronicles.skills_traits(skill_id, trait_id)" + 
                "VALUES (@skill, @trait)" +
                "ON DUPLICATE KEY UPDATE " + 
                "skill_id=@skill, trait_id=@trait", Connection);

        command.Parameters.Add(new MySqlParameter("@skill", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@trait", MySqlDbType.Binary, 16));
        return command;
    }
}
