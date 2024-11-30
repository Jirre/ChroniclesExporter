using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Languages;

[Settings(ETable.Languages)]
public class LanguageSettings : ISettings<LanguageReader, LanguageWriter>
{
    public string FilePath => "Languages";
    public string Url(IRow pData) => "/Languages?id={0}";

    public string LinkIcon(IRow pData) => "language";
    
    public string LinkIconClasses(IRow pData)
    {
        if (pData is not Language language)
            return string.Empty;
        
        return language.Rarity switch
        {
            ERarities.Common => "grey",
            ERarities.Uncommon => "green",
            ERarities.Rare => "blue",
            _ => throw new ArgumentOutOfRangeException($"Unknown Rarity: {language.Rarity}")
        };
    }
    
    public ETable[] Dependencies =>
    [
        ETable.Traits,
        ETable.LanguageScripts
    ];
}
