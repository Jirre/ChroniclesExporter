using ChroniclesExporter.Settings;

namespace ChroniclesExporter.TraitsCategories;

[Settings(ETable.TraitCategories)]
public class TraitCategorySettings : ISettings<TraitCategoryReader, TraitCategoryWriter>
{
    public string FilePath => "Traits-Categories";
    public string Url => "/Traits?id={0}";
    public string LinkClasses => "link-trait-category tooltip tooltip-trait-category";
    public ETable[] Dependencies => [];
}
