using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Actions;

public class Action : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public EActionTypes Type { get; set; }
    public string? Content { get; set; }
}
