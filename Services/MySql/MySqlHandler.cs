using ChroniclesExporter.IO;
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
    private readonly Dictionary<ELink, MySqlWriter<ILink>> _linkWriters =
        new Dictionary<ELink, MySqlWriter<ILink>>();

    /// <summary>
    /// Returns the number of indexed table entries
    /// </summary>
    public static int TableCount => INSTANCE._tableWriters.Count;
    /// <summary>
    /// Returns the number of indexed links
    /// </summary>
    public static int LinkCount => INSTANCE._linkWriters.Count;

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
            if (Activator.CreateInstance(type) is MySqlWriter<IRow> writer)
                INSTANCE._tableWriters.TryAdd((ETable)writer.Id, writer);
        }
    }
    
    private static void LoadLinkWriters()
    {
        Type[] types = TypeUtility.GetTypesBasedOnAbstractParent(typeof(IMySqlLinkWriter));
        foreach (Type type in types)
        {
            if (Activator.CreateInstance(type) is MySqlWriter<ILink> writer)
                INSTANCE._linkWriters.TryAdd((ELink)writer.Id, writer);
        }
    }

    public static bool TryGetWriter(ETable pTable, out MySqlWriter<IRow> pWriter) =>
        INSTANCE._tableWriters.TryGetValue(pTable, out pWriter!);
    
    public static bool TryGetWriter(ELink pLink, out MySqlWriter<ILink> pWriter) =>
        INSTANCE._linkWriters.TryGetValue(pLink, out pWriter!);
    
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
#pragma warning disable CS8601 // Possible null reference assignment.
        pValue = Environment.GetEnvironmentVariable(pKey);
#pragma warning restore CS8601 // Possible null reference assignment.
        return pValue != null;
    }
}
