using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Armor;

[Settings(ETable.Armor)]
public class ArmorSettings : ISettings<Armor, ArmorReader, ArmorWriter>
{
    public string FilePath => "Armors";
    public string Url(IRow pData)
    {
        return "/Armors?id={0}";
    }

    public string LinkIcon(IRow pData)
    {
        return "armor";
    }

    public string LinkIconClasses(IRow pData)
    {
        if (pData is not Armor armor)
            return string.Empty;
        
        return armor.Category switch
        {
            EArmorCategories.unarmored => "grey",
            EArmorCategories.light => "green",
            EArmorCategories.medium => "yellow",
            EArmorCategories.heavy => "red",
            _ => throw new ArgumentOutOfRangeException($"Unknown Group: {armor.Category}")
        };
    }

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
