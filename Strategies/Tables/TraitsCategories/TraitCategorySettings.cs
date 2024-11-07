using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.TraitCategories;

[Settings(ETable.TraitCategories)]
public class TraitCategorySettings : ISettings<TraitCategory, TraitCategoryReader, TraitCategoryWriter>
{
    public string FilePath => "Traits-Categories";

    public string Url(TraitCategory pData)
    {
        return "/Traits?id={0}";
    }

    public string LinkClasses(TraitCategory pData)
    {
        return "link-trait-category tooltip tooltip-trait-category";
    }

    public string LinkIcon(TraitCategory pData)
    {
        return "trait";
    }

    public ETable[] Dependencies => [];
}
