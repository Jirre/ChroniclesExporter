using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Log;

public readonly struct LogEntry(EConsoleMark pMark, ELogTag pTag, string pContext = "")
{
    public EConsoleMark Mark { get; } = pMark;
    public ELogTag Tag { get; } = pTag;
    public string Context { get; } = pContext;
    public DateTime Time { get; } = DateTime.Now;

    public override string ToString() => $"[{Time:HH:mm:ss zz} {Tag.ToString()}; {Context}]";
}
