using ChroniclesExporter.Table;

namespace ChroniclesExporter.Traits;

public class Trait : IRow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public int Priority { get; set; } = 0;
}
