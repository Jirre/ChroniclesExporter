using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitWriter : MySqlTableWriter<Trait>
{
    public override ETable TableId => ETable.Traits;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command = new MySqlCommand("INSERT INTO chronicles.traits(id, name, priority, content)" +
                                                "VALUES (@id, @name, @priority, @content)" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                "id=@id, name=@name, priority=@priority, content=@content",
            Connection);

        command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, byte.MaxValue));
        command.Parameters.Add(new MySqlParameter("@priority", MySqlDbType.Int32));
        command.Parameters.Add(new MySqlParameter("@content", MySqlDbType.Text));
        return command;
    }

    protected override void FillCommand(MySqlCommand pCommand, Trait pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Priority;
        pCommand.Parameters[3].Value = pData.Content;
    }
}
