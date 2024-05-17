using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.MySql;

public class MySqlHandler
{
    public static string SERVER = "localhost";
    public static string PORT = "3004";
    public static string DATABASE = "Chronicles";
    public static string USER_ID = "";
    public static string PASSWORD = "";

    private static readonly MySqlHandler INSTANCE = new MySqlHandler();

    private readonly Dictionary<ETable, MySqlWriter<IRow>> _tableWriters =
        new Dictionary<ETable, MySqlWriter<IRow>>();
}
