using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.CreatureTypes;

public class CreatureType : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public Guid? Parent { get; set; }
}
