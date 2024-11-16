using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Traits)]
public class TraitSettings : ISettings<Trait, TraitReader, TraitWriter>
{
    public string FilePath => "Traits";

    public string Url(IRow pData)
    {
        return "/Traits?id={0}";
    }

    public string LinkClasses(IRow pData)
    {
        return "link-trait tooltip tooltip-trait";
    }

    public string LinkIcon(IRow pData)
    {
        return "trait";
    }

    public ETable[] Dependencies => [];
}
