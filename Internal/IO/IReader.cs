using ChroniclesExporter.Table;

namespace ChroniclesExporter.IO;

public interface IReader
{
    /// <summary>
    /// Read all files
    /// </summary>
    /// <param name="pFiles">Paths to the separate files</param>
    Task Read(string[] pFiles);
}
