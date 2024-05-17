namespace ChroniclesExporter.Table;

public interface IRow
{
    Guid Id { get; set; }
    string Name { get; set; }
    string Content { get; set; }
}
