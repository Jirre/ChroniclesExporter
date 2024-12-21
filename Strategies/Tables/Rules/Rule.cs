using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Rules;

public class Rule : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public ERuleCategory Category { get; set; }
    public short? Priority { get; set; }
}
