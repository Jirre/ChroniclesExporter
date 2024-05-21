using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Abilities;

public class Ability : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}
