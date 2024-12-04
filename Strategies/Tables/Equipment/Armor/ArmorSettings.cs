using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Armor;

[Settings(ETable.Armor)]
public class ArmorSettings : ISettings<ArmorReader, ArmorWriter>
{
    public string FilePath => "Equipment/Armors";
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
            EArmorCategories.Unarmored => "grey",
            EArmorCategories.Light => "green",
            EArmorCategories.Medium => "yellow",
            EArmorCategories.Heavy => "red",
            _ => throw new ArgumentOutOfRangeException($"Unknown Group: {armor.Category}")
        };
    }

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
