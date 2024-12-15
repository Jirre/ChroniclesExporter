using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.CreatureTypes;

[Settings(ETable.CreatureTypes)]
public class CreatureTypeSettings : ISettings<CreatureTypeReader, CreatureTypeWriter>
{
    public string FilePath => "Properties/CreatureTypes";

    public string Url(IRow pData)
    {
        return "/creature-types?id={0}";
    }

    public string LinkIcon(IRow pData)
    {
        return "creature-type";
    }

    public ETable[] Dependencies => [];
}
