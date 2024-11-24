using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Shield;

[Settings(ETable.Shield)]
public class ShieldSettings : ISettings<Shield, ShieldReader, ShieldWriter>
{
    public string FilePath => "Shields";
    public string Url(IRow pData) => "/Armors?id={0}";

    public string LinkIcon(IRow pData) => "armor";
    public string LinkIconClasses(IRow pData) => "blue";

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
