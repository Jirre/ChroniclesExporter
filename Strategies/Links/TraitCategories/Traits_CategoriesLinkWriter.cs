using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Strategy.Links;
using MySqlConnector;

namespace ChroniclesExporter.Strategy.Traits;

// ReSharper disable once InconsistentNaming
public class Traits_CategoriesLinkWriter : MySqlLinkWriter<Link>
{
    public override ELink LinkId => ELink.TraitCategories;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command =
            new MySqlCommand(
                "INSERT INTO chronicles.traits_categories(category_id, trait_id)" + 
                "VALUES (@category, @trait)" +
                "ON DUPLICATE KEY UPDATE " + 
                "category_id=@category, trait_id=@trait", Connection);

        command.Parameters.Add(new MySqlParameter("@category", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@trait", MySqlDbType.Binary, 16));
        return command;
    }
}
