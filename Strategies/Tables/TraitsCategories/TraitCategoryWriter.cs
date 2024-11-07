using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using MySqlConnector;
using Npgsql;
using NpgsqlTypes;

namespace ChroniclesExporter.Strategy.TraitCategories;

public class TraitCategoryWriter : DbTableWriter<TraitCategory>
{
    public override ETable TableId => ETable.TraitCategories;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
                "INSERT INTO chronicles.traitcategories(id, name)" + 
                "VALUES (@id, @name)" +
                "ON DUPLICATE KEY UPDATE " + 
                "id=@id, name=@name");

        command.Parameters.Add("@id", NpgsqlDbType.Uuid);
        command.Parameters.Add("@name", NpgsqlDbType.Varchar, byte.MaxValue);
        return command;
    }

    protected override void FillCommand(NpgsqlCommand pCommand, TraitCategory pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
    }
}
