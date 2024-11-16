using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Settings;

public class SettingsHandler
{
    private static readonly SettingsHandler INSTANCE = new();
    private readonly Dictionary<ETable, ISettings> _settings = new();

    public static int Count => INSTANCE._settings.Count;

    /// <summary>
    ///     Load all Setting Objects within the project
    /// </summary>
    public static void Load()
    {
        Type[] types = ReflectionUtility.GetTypesWithAttribute(typeof(SettingsAttribute));
        foreach (Type type in types)
        {
            ETable table = ((SettingsAttribute) type.GetCustomAttributes(typeof(SettingsAttribute), true).First()).Type;
            if (Activator.CreateInstance(type) is ISettings setting) INSTANCE._settings.TryAdd(table, setting);
        }
    }

    /// <summary>
    ///     Attempts to get the setting object associated with the provided Table Type
    /// </summary>
    public static bool TryGetSettings(ETable pType, out ISettings pSettings)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        return INSTANCE._settings.TryGetValue(pType, out pSettings);
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    /// <summary>
    ///     Attempts to get the setting object associated with the provided Table Type
    /// </summary>
    public static bool TryGetSettings<T>(ETable pType, out ISettings pSettings)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        return INSTANCE._settings.TryGetValue(pType, out pSettings);
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    /// <summary>
    ///     Attempts to get the table-id associated with the provided file-path
    /// </summary>
    public static bool TryGetTable(string pFilePath, out ETable pSettings)
    {
        pSettings = 0;
        foreach (KeyValuePair<ETable, ISettings> kvp in INSTANCE._settings)
        {
            if (kvp.Value.FilePath != pFilePath) continue;
            pSettings = kvp.Key;
            return true;
        }

        return false;
    }
}
