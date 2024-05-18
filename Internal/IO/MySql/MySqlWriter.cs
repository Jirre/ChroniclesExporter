using ChroniclesExporter.MySql;
using ChroniclesExporter.Utility;
using MySqlConnector;

namespace ChroniclesExporter.IO.MySql;

public interface IMySqlWriter : IWriter
{
    bool IsReady { get; }
    Task? Task { get; }
    void LogStatus();
}

public abstract class MySqlWriter<T> : IMySqlWriter, IWriter
{
    protected readonly MySqlConnection Connection = new($"server={MySqlHandler.Server};" +
                                                        $"port={MySqlHandler.Port};" +
                                                        $"database={MySqlHandler.Database};" +
                                                        $"user={MySqlHandler.UserId};" +
                                                        $"password={MySqlHandler.Password}");
    protected T[] QueryQueue;
    protected int LogPosition;
    
    public abstract Enum Id { get; }
    
    public Task? Task { get; private set; }

    public bool IsReady => Task?.IsCompleted ?? false;

    public void LogStatus() =>
        ConsoleUtility.OverwriteMarkedLine(
            Id.ToString(), 
            IsReady ? EConsoleMark.Check : EConsoleMark.Waiting,
            LogPosition);

    public void Prepare(T[] pQueries)
    {
        QueryQueue = pQueries;
        LogPosition = Console.CursorTop;
        ConsoleUtility.WriteMarkedLine(Id.ToString(), EConsoleMark.Waiting);
    }
    
    public Task Write()
    {
        Task = QueryQueue.Length == 0 ? Task.CompletedTask : Task.Run(() => WriteAsync(QueryQueue));
        return Task;
    }

    protected abstract Task WriteAsync(T[] pQueries);
}
