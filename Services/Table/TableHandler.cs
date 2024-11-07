using System.Collections.Concurrent;
using ChroniclesExporter.Log;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Table;

public class TableHandler
{
    private static readonly TableHandler INSTANCE = new();

    private readonly Dictionary<Guid, TableEntry> _guidToTable = new();
    private readonly ConcurrentDictionary<ELink, List<ILink>> _links = new();

    public static TableEntry[] Entries => INSTANCE._guidToTable.Values.ToArray();
    public static Dictionary<ELink, List<ILink>> Links => INSTANCE._links.ToDictionary();

    /// <summary>
    ///     Returns the number of indexed table entries
    /// </summary>
    public static int Count => INSTANCE._guidToTable.Count;

    /// <summary>
    ///     Checks if a table entry with the provided GUID exists
    /// </summary>
    public static bool Contains(Guid pGuid)
    {
        return INSTANCE._guidToTable.ContainsKey(pGuid);
    }

    /// <summary>
    ///     Attempts to get the table entry associated with the GUID provided
    /// </summary>
    public static bool TryGet(Guid pGuid, out TableEntry pEntry)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        return INSTANCE._guidToTable.TryGetValue(pGuid, out pEntry);
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    /// <summary>
    ///     Register a new table entry with the provided file path and table type
    /// </summary>
    public static void Register(string pPath, ETable pTable)
    {
        if (!StringUtility.TryExtractGuidFromString(pPath, out Guid guid))
        {
            LogHandler.Warning(ELogCode.IndexerGuidNotFound, $"Path: {pPath};");
            return;
        }

        if (!INSTANCE._guidToTable.TryAdd(guid, new TableEntry(pTable, pPath)))
            LogHandler.Warning(ELogCode.IndexerGuidCollision, $"Table: {pTable}; Path: {pPath};");
    }

    /// <summary>
    ///     Register a new link entry with the provided link type
    /// </summary>
    public static void RegisterLink(ELink pTable, ILink pLink)
    {
        if (!INSTANCE._links.ContainsKey(pTable))
            INSTANCE._links.TryAdd(pTable, new List<ILink>());
        if (!INSTANCE._links[pTable].Contains(pLink))
            INSTANCE._links[pTable].Add(pLink);
    }
}
