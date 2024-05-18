using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.MySql;

public class MySqlHandler
{
    public static string Server { get; private set; } = "localhost";
    public static string Port { get; private set; } = "3306";
    public static string Database { get; private set; } = "Chronicles";
    public static string UserId { get; set; } = "";
    public static string Password { get; set; } = "";

    private static readonly MySqlHandler INSTANCE = new MySqlHandler();

    private readonly Dictionary<ETable, MySqlWriter<IRow>> _tableWriters =
        new Dictionary<ETable, MySqlWriter<IRow>>();
    private readonly Dictionary<ELink, MySqlLinkWriter<ILink>> _linkWriters =
        new Dictionary<ELink, MySqlLinkWriter<ILink>>();

    /// <summary>
    /// Returns the number of indexed table entries
    /// </summary>
    public static int TableCount => INSTANCE._tableWriters.Count;
    /// <summary>
    /// Returns the number of indexed links
    /// </summary>
    public static int LinkCount => INSTANCE._linkWriters.Count;

    /// <summary>
    /// Sets the connection variables to any provided environment variables
    /// </summary>
    public static void SetEnvironmentVariables()
    {
        if (TryGetEnvironmentVariable("MYSQL_SERVER", out string server)) Server = server;
        if (TryGetEnvironmentVariable("MYSQL_PORT", out string port)) Port = port;
        if (TryGetEnvironmentVariable("MYSQL_DATABASE", out string database)) Database = database;
        if (TryGetEnvironmentVariable("MYSQL_USER_ID", out string userId)) UserId = userId;
        if (TryGetEnvironmentVariable("MYSQL_PASSWORD", out string password)) Password = password;
    }

    private static bool TryGetEnvironmentVariable(string pKey, out string pValue)
    {
        pValue = Environment.GetEnvironmentVariable(pKey);
        return pValue != null;
    }

    /// <summary>
    /// Load all Table- and Link-Writers within the project
    /// </summary>
    public static void Load()
    {
        LoadTableWriters();
        LoadLinkWriters();
    }

    private static void LoadTableWriters()
    {
        Type[] types = TypeUtility.GetTypesBasedOnAbstractParent(typeof(IMySqlTableWriter));
        foreach (Type type in types)
        {
            if (Activator.CreateInstance(type) is MySqlTableWriter<IRow> writer)
                INSTANCE._tableWriters.TryAdd(writer.TableId, writer);
        }
    }
    
    private static void LoadLinkWriters()
    {
        Type[] types = TypeUtility.GetTypesBasedOnAbstractParent(typeof(IMySqlLinkWriter));
        foreach (Type type in types)
        {
            if (Activator.CreateInstance(type) is MySqlLinkWriter<ILink> writer)
                INSTANCE._linkWriters.TryAdd(writer.LinkId, writer);
        }
    }
}
