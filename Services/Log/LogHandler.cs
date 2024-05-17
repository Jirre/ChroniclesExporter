using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Log;

public class LogHandler
{
    private static readonly LogHandler INSTANCE = new LogHandler();
    private readonly List<LogEntry> _logList = new List<LogEntry>();

    public static int InfoCount => INSTANCE._logList.Count(e => e.Mark == EConsoleMark.Info);
    public static int WarningCount => INSTANCE._logList.Count(e => e.Mark == EConsoleMark.Warning);
    public static int ErrorCount => INSTANCE._logList.Count(e => e.Mark == EConsoleMark.Error);
    
    public static void Log(ELogTag pTag) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Info, pTag));
    public static void Log(ELogTag pTag, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Info, pTag, pMessage));
    
    public static void Warning(ELogTag pTag) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Warning, pTag));
    public static void Warning(ELogTag pTag, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Warning, pTag, pMessage));
    
    public static void Error(ELogTag pTag) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Error, pTag));
    public static void Error(ELogTag pTag, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Error, pTag, pMessage));

    public static void Print(EConsoleMark pMark)
    {
        foreach (LogEntry log in INSTANCE._logList)
        {
            if ((log.Mark & pMark) != 0)
                ConsoleUtility.WriteMarkedLine(log.ToString(), log.Mark);
        }
    }
}
