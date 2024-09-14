using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Traits;

public class SkillWriter : MySqlTableWriter<Skill>
{
    public override ETable TableId => ETable.Skills;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command = new MySqlCommand("INSERT INTO chronicles.skills(id, name, ability, content)" +
                                                "VALUES (@id, @name, @ability, @content)" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                "id=@id, name=@name, ability=@ability, content=@content",
            Connection);

        command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, byte.MaxValue));
        command.Parameters.Add(new MySqlParameter("@ability", MySqlDbType.Enum));
        command.Parameters.Add(new MySqlParameter("@content", MySqlDbType.Text));
        return command;
    }

    protected override void FillCommand(MySqlCommand pCommand, Skill pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Ability.ToString().ToLower();
        pCommand.Parameters[3].Value = pData.Content == null ? null : MySqlHelper.EscapeString(pData.Content);
    }
}
