using System.Data;
using ChroniclesExporter.Database;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Strategy.Links;
using MySqlConnector;
using Npgsql;

namespace ChroniclesExporter.Strategy.Traits;

// ReSharper disable once InconsistentNaming
public class Trait_CategoriesLinkWriter : DbLinkWriter<Link>
{
    public override ELink LinkId => ELink.TraitCategories;
    protected override NpgsqlCommand BuildCommand()
    {
        NpgsqlCommand command = DbHandler.DataSource.CreateCommand(
                "INSERT INTO chronicles.traits_categories(trait_id, category_id)" + 
                "VALUES (@trait, @category)" +
                "ON DUPLICATE KEY UPDATE " + 
                "trait_id=@trait, category_id=@category");

        command.Parameters.Add(new NpgsqlParameter("@trait", DbType.Binary, 16));
        command.Parameters.Add(new NpgsqlParameter("@category", DbType.Binary, 16));
        return command;
    }
}
