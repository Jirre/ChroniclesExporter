using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Abilities;

public class AbilityWriter : MySqlTableWriter<Ability>
{
    public override ETable TableId => ETable.Abilities;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command = new MySqlCommand("INSERT INTO chronicles.abilities(id, name, content)" +
                                                "VALUES (@id, @name, @content)" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                "id=@id, name=@name, content=@content",
            Connection);

        command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, byte.MaxValue));
        command.Parameters.Add(new MySqlParameter("@content", MySqlDbType.Text));
        return command;
    }

    protected override void FillCommand(MySqlCommand pCommand, Ability pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Content == null ? null : MySqlHelper.EscapeString(pData.Content);
    }
}
