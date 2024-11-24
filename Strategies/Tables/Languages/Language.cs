using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Languages;

public class Language : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public ERarities? Rarity { get; set; }
    public Guid? Script { get; set; }
}
