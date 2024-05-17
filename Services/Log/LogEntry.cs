namespace ChroniclesExporter.Log;

public readonly struct LogEntry(ELogTag pTag, string pContext = "")
{
    public ELogTag Tag { get; } = pTag;
    public string Context { get; } = pContext;
    public DateTime Time { get; } = DateTime.Now;

    public override string ToString() => $"[{Time:HH:mm:ss zz} {Tag.ToString()}; {Context}]";
}
