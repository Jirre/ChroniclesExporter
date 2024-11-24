using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Languages;

public class Script : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}
