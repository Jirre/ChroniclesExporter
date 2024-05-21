using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.TraitCategories;

public class TraitCategoryWriter : MySqlTableWriter<TraitCategory>
{
    public override ETable TableId => ETable.TraitCategories;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command =
            new MySqlCommand(
                "INSERT INTO chronicles.TraitCategories(id, name)" + 
                "VALUES (@id, @name)" +
                "ON DUPLICATE KEY UPDATE " + 
                "id=@id, name=@name", Connection);

        command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, byte.MaxValue));
        return command;
    }

    protected override void FillCommand(MySqlCommand pCommand, TraitCategory pData)
    {
        pCommand.Parameters[0].Value = pData.Id.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Name;
    }
}
