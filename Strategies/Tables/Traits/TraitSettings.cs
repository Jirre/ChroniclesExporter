using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Traits)]
public class TraitSettings : ISettings<TraitReader, TraitWriter>
{
    public string FilePath => "Traits";
    public string Url => "/Traits?id={0}";
    public string LinkClasses => "link-trait tooltip tooltip-trait";
    public string LinkIcon => "trait";
    
    public ETable[] Dependencies =>
    [
        ETable.TraitCategories
    ];
}
