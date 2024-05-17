namespace ChroniclesExporter.Settings;

public class SettingsAttribute(ETable pType) : Attribute
{
    public ETable Type { get; } = pType;
}
