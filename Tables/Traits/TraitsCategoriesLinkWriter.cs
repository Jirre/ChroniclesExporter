using ChroniclesExporter.IO.MySql;
using MySqlConnector;

namespace ChroniclesExporter.Traits;

public class TraitsCategoriesLinkWriter : MySqlLinkWriter<Link>
{
    public override ELink LinkId => ELink.TraitCategories;
    protected override MySqlCommand BuildCommand()
    {
        MySqlCommand command = new MySqlCommand("INSERT INTO chronicles.traits_categories(category_id, trait_id)" +
                                                "VALUES (@category, @trait)" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                "category_id=@category, trait_id=@trait",
            Connection);

        command.Parameters.Add(new MySqlParameter("@category", MySqlDbType.Binary, 16));
        command.Parameters.Add(new MySqlParameter("@trait", MySqlDbType.Binary, 16));
        return command;
    }
}
