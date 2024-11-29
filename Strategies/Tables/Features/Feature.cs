using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Features;

public class Feature : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public short? Index { get; set; }
}
