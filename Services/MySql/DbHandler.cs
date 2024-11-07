using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using Npgsql;

namespace ChroniclesExporter.Database;

public class DbHandler
{
    private static string _host = "localhost";
    private static string _port = "5432";
    private static string _database = "chronicles";
    public static string Username { get; set; } = "";
    public static string Password { get; set; } = "";

    public static NpgsqlDataSource DataSource { get; private set; } = null!;

    private static readonly DbHandler INSTANCE = new DbHandler();
    
    private readonly Dictionary<ETable, DbWriter<IRow>> _tableWriters =
        new Dictionary<ETable, DbWriter<IRow>>();
    private readonly Dictionary<ELink, DbWriter<ILink>> _linkWriters =
        new Dictionary<ELink, DbWriter<ILink>>();

    public static void Setup()
    {
        DataSource = NpgsqlDataSource.Create(
            $"Host={_host};" +
            $"Port={_port};" +
            $"Database={_database};" +
            $"Username={Username}" +
            $"Password={Password}");
    }
    
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
        Type[] types = TypeUtility.GetTypesBasedOnAbstractParent(typeof(ITableWriter));
        foreach (Type type in types)
        {
            if (Activator.CreateInstance(type) is DbWriter<IRow> writer)
                INSTANCE._tableWriters.TryAdd((ETable)writer.Id, writer);
        }
    }
    
    private static void LoadLinkWriters()
    {
        Type[] types = TypeUtility.GetTypesBasedOnAbstractParent(typeof(ILinkWriter));
        foreach (Type type in types)
        {
            if (Activator.CreateInstance(type) is DbWriter<ILink> writer)
                INSTANCE._linkWriters.TryAdd((ELink)writer.Id, writer);
        }
    }

    public static bool TryGetWriter(ETable pTable, out DbWriter<IRow> pWriter) =>
        INSTANCE._tableWriters.TryGetValue(pTable, out pWriter!);
    
    public static bool TryGetWriter(ELink pLink, out DbWriter<ILink> pWriter) =>
        INSTANCE._linkWriters.TryGetValue(pLink, out pWriter!);
    
    /// <summary>
    /// Sets the connection variables to any provided environment variables
    /// </summary>
    public static void SetEnvironmentVariables()
    {
        if (TryGetEnvironmentVariable("DB_SERVER", out string server)) _host = server;
        if (TryGetEnvironmentVariable("DB_PORT", out string port)) _port = port;
        if (TryGetEnvironmentVariable("DB_DATABASE", out string database)) _database = database;
        if (TryGetEnvironmentVariable("DB_USER_ID", out string userId)) Username = userId;
        if (TryGetEnvironmentVariable("DB_PASSWORD", out string password)) Password = password;
    }

    private static bool TryGetEnvironmentVariable(string pKey, out string pValue)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        pValue = Environment.GetEnvironmentVariable(pKey);
#pragma warning restore CS8601 // Possible null reference assignment.
        return pValue != null;
    }
}
