namespace ChroniclesExporter.IO;

public interface IWriter : ITaskHandler
{
    /// <summary>
    /// Write all files currently stored in the MySqlHandler
    /// </summary>
    Task Write();
}
