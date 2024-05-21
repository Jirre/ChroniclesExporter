namespace ChroniclesExporter.Settings;

[AttributeUsage(AttributeTargets.Class)]
public class SettingsAttribute(ETable pType) : Attribute
{
    public ETable Type { get; } = pType;
}
