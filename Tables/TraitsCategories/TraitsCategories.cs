using ChroniclesExporter.Table;

namespace ChroniclesExporter.TraitsCategories;

public class TraitsCategories : IRow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
}
