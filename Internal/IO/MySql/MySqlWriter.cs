using ChroniclesExporter.MySql;
using MySqlConnector;
using Spectre.Console;

namespace ChroniclesExporter.IO.MySql;

public interface IMySqlWriter : IWriter
{
    Task? Task { get; }
}

public abstract class MySqlWriter<T> : IMySqlWriter
{
    protected readonly MySqlConnection Connection = new($"server={MySqlHandler.Server};" +
                                                        $"port={MySqlHandler.Port};" +
                                                        $"database={MySqlHandler.Database};" +
                                                        $"user={MySqlHandler.UserId};" +
                                                        $"password={MySqlHandler.Password}");
    protected T[] QueryQueue;
    
    public abstract Enum Id { get; }
    
    public Task? Task { get; private set; }

    public bool IsReady => Task?.IsCompleted ?? false;

    public int Progress { get; protected set; }
    
    public void Prepare(T[] pQueries)
    {
        QueryQueue = pQueries;
    }
    
    public Task Write()
    {
        Task = QueryQueue.Length == 0 ? Task.CompletedTask : Task.Run(() => WriteAsync(QueryQueue));
        return Task;
    }

    protected abstract Task WriteAsync(T[] pQueries);
}
