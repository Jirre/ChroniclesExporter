using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Traits)]
public class TraitSettings : ISettings<Trait, TraitReader, TraitWriter>
{
    public string FilePath => "Traits";
    public string Url(Trait pData) => "/Traits?id={0}";
    public string LinkClasses(Trait pData) => "link-trait tooltip tooltip-trait";
    public string LinkIcon(Trait pData) => "trait";
    
    public ETable[] Dependencies =>
    [
        ETable.TraitCategories
    ];
}
