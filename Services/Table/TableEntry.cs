namespace ChroniclesExporter.Table;

public class TableEntry(ETable pId, string pPath)
{
    public ETable Id { get; } = pId;
    public string Path { get; } = pPath;
    public IRow? Row { get; set; }
}
