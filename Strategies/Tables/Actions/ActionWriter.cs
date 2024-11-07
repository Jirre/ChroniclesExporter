using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using Npgsql;
using NpgsqlTypes;

namespace ChroniclesExporter.Strategy.Actions;

public class ActionWriter : DbTableWriter<Action>
{
    public override ETable TableId => ETable.Actions;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
            "INSERT INTO chronicles.actions(id, name, details, type, content)" +
            "VALUES (@id, @name, @details, @type, @content)" +
            "ON DUPLICATE KEY UPDATE " +
            "id=@id, name=@name, details=@details, type=@type, content=@content");

        command.Parameters.Add("@id", NpgsqlDbType.Uuid, 16);
        command.Parameters.Add("@name", NpgsqlDbType.Varchar, byte.MaxValue);
        command.Parameters.Add("@details", NpgsqlDbType.Varchar, 2048);
        command.Parameters.Add(new NpgsqlParameter { ParameterName = "@type", DataTypeName = "actionType" });
        command.Parameters.Add("@content", NpgsqlDbType.Text);
        return command;
    }

    protected override void FillCommand(NpgsqlCommand pCommand, Action pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
        pCommand.Parameters[2].Value = pData.Details;
        pCommand.Parameters[3].Value = pData.Type.ToString().ToLower();
        pCommand.Parameters[4].Value = pData.Content;
    }
}
