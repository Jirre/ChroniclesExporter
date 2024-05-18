namespace ChroniclesExporter.IO;

public interface IWriter
{
    /// <summary>
    /// Write all files currently stored in the MySqlHandler
    /// </summary>
    /// <param name="pFiles">Paths to the separate files</param>
    Task Write();
}
