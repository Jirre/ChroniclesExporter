namespace ChroniclesExporter.Utility;

public class IOUtility
{
    public static string GetRoot()
    {
#if DEBUG
        return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
#else
        return path = Environment.CurrentDirectory;
#endif
    }
    
    public static string GetDataRoot()
    {
#if DEBUG
        return Path.Combine(GetRoot(), "Data");
#else
        Return GetRoot();
#endif
    }
}
