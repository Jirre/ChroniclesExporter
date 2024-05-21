using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Conditions;

public class Condition : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}
