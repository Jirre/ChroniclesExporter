using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Races;

[Settings(ETable.Races)]
public class RaceSettings : ISettings<Race, RaceReader, RaceWriter>
{
    public string FilePath => "Races";

    public ETable[] Dependencies =>
    [
        ETable.CreatureTypes,
        ETable.Features,
        ETable.Languages,
        ETable.Traits,
    ];

    public string Url(IRow pData) => "/Races/{1}";

    public string LinkIcon(IRow pData) => "race";
    public string LinkIconClasses(IRow pData)
    {
        if (pData is not Race race)
            return string.Empty;
        
        return race.Rarity switch
        {
            ERarities.common => "grey",
            ERarities.uncommon => "green",
            ERarities.rare => "blue",
            _ => throw new ArgumentOutOfRangeException($"Unknown Rarity: {race.Rarity}")
        };
    }
}
