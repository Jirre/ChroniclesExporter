
using System.Data;
using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using MySqlConnector;
using Npgsql;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Skill_Traits : DbLinkWriter<Link>
{
    public override ELink LinkId => ELink.SkillTraits;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
                "INSERT INTO chronicles.skills_traits(skill_id, trait_id)" + 
                "VALUES (@skill, @trait)" +
                "ON DUPLICATE KEY UPDATE " + 
                "skill_id=@skill, trait_id=@trait");

        command.Parameters.Add(new NpgsqlParameter("@skill", DbType.Binary, 16));
        command.Parameters.Add(new NpgsqlParameter("@trait", DbType.Binary, 16));
        return command;
    }
}
