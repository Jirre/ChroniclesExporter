using ChroniclesExporter.Table;

namespace ChroniclesExporter.IO.MySql;

public interface IMySqlLinkWriter
{
    public ELink LinkId { get; }
}

public abstract class MySqlLinkWriter<T> : MySqlWriter<ILink>, IMySqlLinkWriter
    where T : ILink
{
    public abstract ELink LinkId { get; protected set; }
    public override Enum Id => LinkId;

    protected override async Task WriteAsync(ILink[] pQueries)
    {
        
    }
}