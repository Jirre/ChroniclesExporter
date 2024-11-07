namespace ChroniclesExporter.IO.Database;

public abstract class DbWriter<T> : IWriter
{
    private T[]? _queries;

    public abstract Enum Id { get; }

    public Task? Task { get; private set; }

    public bool IsReady => Task?.IsCompleted ?? false;

    public int Progress { get; protected set; }
    public int TaskCount => _queries?.Length ?? 0;

    public Task Write()
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
