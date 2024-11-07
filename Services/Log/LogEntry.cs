using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Log;

public readonly struct LogEntry(EConsoleMark pMark, ELogCode pCode, string pContext = "")
{
    public EConsoleMark Mark { get; } = pMark;
    public ELogCode Code { get; } = pCode;
    public string Context { get; } = pContext;
    public DateTime Time { get; } = DateTime.Now;

    public override string ToString()
    {
        return $"[{Time:HH:mm:ss zz} {Code.ToString()}; {Context}]";
    }
}
