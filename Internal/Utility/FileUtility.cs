using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Utility;

public static class FileUtility
{
    /// <summary>
    ///     Returns the project's root folder
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
    ///     Returns the project's data folder
    /// </summary>
    public static string GetDataRoot()
    {
#if DEBUG
        return Path.Combine(GetRoot(), "Data");
#else
        return GetRoot();
#endif
    }

    public static bool TryGetTypeFromPath(string pPath, out ETable pType, bool pLogError = true)
    {
        string? folder = Directory.GetParent(pPath)?.FullName;
        if (folder?.TryTrimStart(GetDataRoot(), out string subPath) ?? false)
        {
            subPath = subPath.Replace('\\', '/').Trim('/');
            if (!string.IsNullOrWhiteSpace(subPath) && SettingsHandler.TryGetTable(subPath, out pType))
                return true;
        }

        if (pLogError) LogHandler.Warning(ELogCode.IndexerPathNotFound, pPath);
        pType = 0;
        return false;
    }
}
