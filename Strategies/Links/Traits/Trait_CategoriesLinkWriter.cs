using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Strategy.Links;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Traits;

// ReSharper disable once InconsistentNaming
public class Trait_CategoriesLinkWriter : MySqlLinkWriter<Link>
{
    public override ELink LinkId => ELink.TraitCategories;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command =
            new MySqlCommand(
                "INSERT INTO chronicles.traits_categories(trait_id, category_id)" + 
                "VALUES (@trait, @category)" +
                "ON DUPLICATE KEY UPDATE " + 
                "trait_id=@trait, category_id=@category", Connection);

        command.Parameters.Add(new MySqlParameter("@trait", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@category", MySqlDbType.Binary, 16));
        return command;
    }
}
