using System.Data;
using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Condition_Traits : DbLinkWriter<Link>
{
    public override ELink LinkId => ELink.ConditionTraits;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
            "INSERT INTO chronicles.conditions_traits(condition_id, trait_id)" +
            "VALUES (@condition, @trait)" +
            "ON DUPLICATE KEY UPDATE " +
            "condition_id=@condition, trait_id=@trait");

        command.Parameters.Add(new NpgsqlParameter("@condition", DbType.Binary, 16));
        command.Parameters.Add(new NpgsqlParameter("@trait", DbType.Binary, 16));
        return command;
    }
}