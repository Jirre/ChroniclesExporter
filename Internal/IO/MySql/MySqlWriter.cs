using ChroniclesExporter.MySql;
using MySqlConnector;

namespace ChroniclesExporter.IO.MySql;

public abstract class MySqlWriter<T> : IWriter
{
    protected readonly MySqlConnection Connection = new MySqlConnection($"server={MySqlHandler.Server};" +
                                                                        $"port={MySqlHandler.Port};" +
                                                                        $"database={MySqlHandler.Database};" +
                                                                        $"user={MySqlHandler.UserId};" +
                                                                        $"password={MySqlHandler.Password}");
    private T[]? _queries;
    
    public abstract Enum Id { get; }
    
    public Task? Task { get; private set; }

    public bool IsReady => Task?.IsCompleted ?? false;

    public int Progress { get; protected set; }
    public int TaskCount => _queries?.Length ?? 0;
    
    public void Prepare(T[] pQueries)
    {
        _queries = pQueries;
    }
    
    public Task Write()
    {
        if (_queries == null)
            return Task.CompletedTask;
        Task = _queries?.Length == 0 ? Task.CompletedTask : Task.Run(() => WriteAsync(_queries!));
        return Task;
    }

    protected abstract Task WriteAsync(T[] pQueries);
}
