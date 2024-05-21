using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Utility;

public static class FileUtility
{
    /// <summary>
    /// Returns the project's root folder
    /// </summary>
    public static string GetRoot()
    {
#if DEBUG
        return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "NULL";
#else
        return Path.GetFullPath(Environment.CurrentDirectory);
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
        return GetRoot();
#endif
    }
    
    public static bool TryGetTypeFromPath(string pPath, out ETable pType, int pMaxDepth = 3, bool pLogError = true)
    {
        string path = pPath;
        for (int index = 0; 
             (index < 3 || pMaxDepth <= 0) && path != Path.GetPathRoot(pPath); 
             index++)
        {
            path = Directory.GetParent(path)?.FullName ?? "NULL";
            string fileName = Path.GetFileName(path);
            if (!string.IsNullOrWhiteSpace(fileName) && SettingsHandler.TryGetTable(fileName, out pType))
                return true;
        }
        if (pLogError) LogHandler.Warning(ELogCode.IndexerPathNotFound, pPath);
        pType = 0;
        return false;
    }
}
