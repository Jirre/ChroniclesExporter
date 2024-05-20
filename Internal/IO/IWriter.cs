namespace ChroniclesExporter.IO;

public interface IWriter
{
    /// <summary>
    /// Write all files currently stored in the MySqlHandler
    /// </summary>
    Task Write();
    
    bool IsReady { get; }
}
