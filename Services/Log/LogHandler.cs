namespace ChroniclesExporter.Log;

public class LogHandler
{
    private static readonly LogHandler INSTANCE = new LogHandler();
    private readonly List<LogEntry> _logList = new List<LogEntry>();
    public static ICollection<LogEntry> Entries => INSTANCE._logList;

    public static void Add(LogEntry pEntry) => INSTANCE._logList.Add(pEntry);
    public static void Add(ELogTag pTag) => INSTANCE._logList.Add(new LogEntry(pTag));
    public static void Add(ELogTag pTag, string pMessage) => INSTANCE._logList.Add(new LogEntry(pTag, pMessage));
}
