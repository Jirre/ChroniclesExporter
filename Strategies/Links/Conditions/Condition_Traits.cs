using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Condition_Traits : MySqlLinkWriter<Link>
{
    public override ELink LinkId => ELink.ConditionTraits;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command =
            new MySqlCommand(
                "INSERT INTO chronicles.conditions_traits(condition_id, trait_id)" + 
                "VALUES (@condition, @trait)" +
                "ON DUPLICATE KEY UPDATE " + 
                "condition_id=@condition, trait_id=@trait", Connection);

        command.Parameters.Add(new MySqlParameter("@condition", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@trait", MySqlDbType.Binary, 16));
        return command;
    }
}
