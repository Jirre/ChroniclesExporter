using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.TraitCategories;

public class TraitCategory : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}
