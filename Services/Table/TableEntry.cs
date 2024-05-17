namespace ChroniclesExporter.Table;

public class TableEntry(ETable pId, string pPath)
{
    public ETable Id { get; set; } = pId;
    public string Path { get; set; } = pPath;
    public IRow Row { get; set; }
}
