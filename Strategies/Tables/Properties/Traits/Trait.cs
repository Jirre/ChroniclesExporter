using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

public class Trait : IRow
{
    public int Priority { get; set; }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public ETraitCategories[]? Categories { get; set; }
    public string Icon { get; set; }
    public string Class { get; set; }
}
