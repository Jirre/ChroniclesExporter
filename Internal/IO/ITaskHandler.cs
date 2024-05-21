namespace ChroniclesExporter.IO;

public interface ITaskHandler
{
    int Progress { get; }
    int TaskCount { get; }
}
