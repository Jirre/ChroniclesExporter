using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using Npgsql;
using NpgsqlTypes;

namespace ChroniclesExporter.Strategy.Traits;

public class SkillWriter : DbTableWriter<Skill>
{
    public override ETable TableId => ETable.Skills;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
            "INSERT INTO chronicles.skills(id, name, ability, content)" +
            "VALUES (@id, @name, @ability, @content)" +
            "ON DUPLICATE KEY UPDATE " +
            "id=@id, name=@name, ability=@ability, content=@content");

        command.Parameters.Add("@id", NpgsqlDbType.Uuid);
        command.Parameters.Add("@name", NpgsqlDbType.Varchar, byte.MaxValue);
        command.Parameters.Add(new NpgsqlParameter { ParameterName = "@ability", DataTypeName = "abilities" });
        command.Parameters.Add("@content", NpgsqlDbType.Text);
        return command;
    }

    protected override void FillCommand(NpgsqlCommand pCommand, Skill pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Ability.ToString().ToLower();
        pCommand.Parameters[3].Value = pData.Content;
    }
}
