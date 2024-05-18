namespace ChroniclesExporter.Utility;

public class IOUtility
{
    /// <summary>
    /// Returns the project's root folder
    /// </summary>
    public static string GetRoot()
    {
#if DEBUG
        return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
#else
        return path = Environment.CurrentDirectory;
#endif
    }
    
    /// <summary>
    /// Returns the project's data folder
    /// </summary>
    public static string GetDataRoot()
    {
#if DEBUG
        return Path.Combine(GetRoot(), "Data");
#else
        Return GetRoot();
#endif
    }
}
