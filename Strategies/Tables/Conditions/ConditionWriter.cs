using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using Npgsql;
using NpgsqlTypes;

namespace ChroniclesExporter.Strategy.Conditions;

public class ConditionWriter : DbTableWriter<Condition>
{
    public override ETable TableId => ETable.Conditions;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
            "INSERT INTO chronicles.conditions(id, name, content)" +
             "VALUES (@id, @name, @content)" +
             "ON DUPLICATE KEY UPDATE " +
             "id=@id, name=@name, content=@content");

        command.Parameters.Add("@id", NpgsqlDbType.Uuid);
        command.Parameters.Add("@name", NpgsqlDbType.Varchar, byte.MaxValue);
        command.Parameters.Add("@content", NpgsqlDbType.Text);
        return command;
    }

    protected override void FillCommand(NpgsqlCommand pCommand, Condition pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Content;
    }
}
