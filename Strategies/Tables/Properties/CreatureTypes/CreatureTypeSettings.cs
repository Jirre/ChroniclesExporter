using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.CreatureTypes;

[Settings(ETable.CreatureTypes)]
public class CreatureTypeSettings : ISettings<CreatureType, CreatureTypeReader, CreatureTypeWriter>
{
    public string FilePath => "CreatureTypes";

    public string Url(IRow pData)
    {
        return "/CreatureTypes?id={0}";
    }

    public string LinkIcon(IRow pData)
    {
        return "creature-type";
    }

    public ETable[] Dependencies => [];
}
