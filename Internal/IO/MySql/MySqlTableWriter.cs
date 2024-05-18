using ChroniclesExporter.Table;

namespace ChroniclesExporter.IO.MySql;

public interface IMySqlTableWriter
{
    public ETable TableId { get; }
}

public abstract class MySqlTableWriter<T> : MySqlWriter<IRow>, IMySqlTableWriter
    where T : IRow
{
    public abstract ETable TableId { get; protected set; }
    public override Enum Id => TableId;

    protected override async Task WriteAsync(IRow[] pQueries)
    {
        
    }
}