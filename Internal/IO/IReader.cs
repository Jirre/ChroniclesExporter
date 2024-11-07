namespace ChroniclesExporter.IO;

public interface IReader : ITaskHandler
{
    /// <summary>
    ///     Read all files
    /// </summary>
    /// <param name="pFiles">Paths to the separate files</param>
    Task Read(string[] pFiles);
}
