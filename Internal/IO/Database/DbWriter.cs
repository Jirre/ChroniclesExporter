namespace ChroniclesExporter.IO.Database;

public abstract class DbWriter : IWriter
{
    public abstract Enum Id { get; }
    
    public Task? Task { get; protected set; }
    public int Progress { get; protected set; }
    public abstract int TaskCount { get; }
    public abstract Task Write();
}
public abstract class DbWriter<T> : DbWriter
{
    private T[]? _queries;
    public sealed override int TaskCount => _queries?.Length ?? 0;

    public sealed override Task Write()
    {
        if (_queries == null)
            return Task.CompletedTask;
        Task = _queries?.Length == 0 ? Task.CompletedTask : Task.Run(() => WriteAsync(_queries!));
        return Task;
    }

    public void Prepare(T[] pQueries)
    {
        _queries = pQueries;
    }

    protected abstract Task WriteAsync(T[] pQueries);
}
