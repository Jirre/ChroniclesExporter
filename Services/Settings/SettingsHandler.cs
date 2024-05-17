namespace ChroniclesExporter.Settings;

public class SettingsHandler
{
    private static readonly SettingsHandler INSTANCE = new SettingsHandler();
    private readonly Dictionary<ETable, ISettings> _settings = new Dictionary<ETable, ISettings>();

    public static bool TryGetSettings(ETable pType, out ISettings pSettings) =>
        INSTANCE._settings.TryGetValue(pType, out pSettings);
}
