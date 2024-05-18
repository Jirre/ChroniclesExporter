namespace ChroniclesExporter.Table;

public interface IRow
{
    Guid Id { get; set; }
    string Name { get; set; }
    /// <summary>
    /// HTML Content as parsed my markdig
    /// </summary>
    string Content { get; set; }
}
