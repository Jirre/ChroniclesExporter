using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Log;

public class LogHandler
{
    private static readonly LogHandler INSTANCE = new LogHandler();
    private readonly List<LogEntry> _logList = new List<LogEntry>();

    /// <summary>
    /// Returns the number of logged information
    /// </summary>
    public static int InfoCount => INSTANCE._logList.Count(pEntry => pEntry.Mark == EConsoleMark.Info);
    /// <summary>
    /// Returns the number of logged warnings
    /// </summary>
    public static int WarningCount => INSTANCE._logList.Count(pEntry => pEntry.Mark == EConsoleMark.Warning);
    /// <summary>
    /// Returns the number of logged errors
    /// </summary>
    public static int ErrorCount => INSTANCE._logList.Count(pEntry => pEntry.Mark == EConsoleMark.Error);
    
    /// <summary>
    /// Logs an information Code
    /// </summary>
    public static void Log(ELogCode pCode) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Info, pCode));
    /// <summary>
    /// Logs an information Code and message
    /// </summary>
    public static void Log(ELogCode pCode, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Info, pCode, pMessage));
    
    /// <summary>
    /// Logs a warning Code
    /// </summary>
    public static void Warning(ELogCode pCode) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Warning, pCode));
    /// <summary>
    /// Logs a warning Code and message
    /// </summary>
    public static void Warning(ELogCode pCode, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Warning, pCode, pMessage));
    
    /// <summary>
    /// Logs an error Code
    /// </summary>
    public static void Error(ELogCode pCode) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Error, pCode));
    /// <summary>
    /// Logs an error Code and message
    /// </summary>
    public static void Error(ELogCode pCode, string pMessage) => INSTANCE._logList.Add(new LogEntry(EConsoleMark.Error, pCode, pMessage));

    /// <summary>
    /// Prints every logged code and message of the given types to the console
    /// </summary>
    public static void Print(EConsoleMark pMark)
    {
        foreach (LogEntry log in INSTANCE._logList)
        {
            if ((log.Mark & pMark) != 0)
                ConsoleUtility.WriteMarkedLine(log.ToString(), log.Mark);
        }
    }
}
