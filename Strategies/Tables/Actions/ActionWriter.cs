using ChroniclesExporter.IO.MySql;
using MySqlConnector;
using Action = ChroniclesExporter.Strategy.Actions.Action;

namespace ChroniclesExporter.Strategy.Actions;

public class ActionWriter : MySqlTableWriter<Action>
{
    public override ETable TableId => ETable.Actions;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command = new MySqlCommand("INSERT INTO chronicles.actions(id, name, details, type, content)" +
                                                "VALUES (@id, @name, @details, @type, @content)" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                "id=@id, name=@name, details=@details, type=@type, content=@content",
            Connection);

        command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, byte.MaxValue));
        command.Parameters.Add(new MySqlParameter("@details", MySqlDbType.VarChar, 2048));
        command.Parameters.Add(new MySqlParameter("@type", MySqlDbType.Enum));
        command.Parameters.Add(new MySqlParameter("@content", MySqlDbType.Text));
        return command;
    }

    protected override void FillCommand(MySqlCommand pCommand, Action pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Details;
        pCommand.Parameters[3].Value = pData.Type.ToString().ToLower();
        pCommand.Parameters[4].Value = pData.Content;
    }
}
