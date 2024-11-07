using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using MySqlConnector;
using Npgsql;
using NpgsqlTypes;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitWriter : DbTableWriter<Trait>
{
    public override ETable TableId => ETable.Traits;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
            "INSERT INTO chronicles.traits(id, name, priority, content)" +
             "VALUES (@id, @name, @priority, @content)" +
             "ON DUPLICATE KEY UPDATE " +
             "id=@id, name=@name, priority=@priority, content=@content");

        command.Parameters.Add("@id", NpgsqlDbType.Uuid);
        command.Parameters.Add("@name", NpgsqlDbType.Varchar, byte.MaxValue);
        command.Parameters.Add("@priority", NpgsqlDbType.Integer);
        command.Parameters.Add("@content", NpgsqlDbType.Text);
        return command;
    }

    protected override void FillCommand(NpgsqlCommand pCommand, Trait pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Priority;
        pCommand.Parameters[3].Value = pData.Content;
    }
}
