using System.Collections.Concurrent;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Table;

public class TableHandler
{
    private static readonly TableHandler INSTANCE = new TableHandler();

    private readonly Dictionary<Guid, TableEntry> _guidToTable = new Dictionary<Guid, TableEntry>();
    private readonly ConcurrentDictionary<ELink, List<ILink>> _links = new ConcurrentDictionary<ELink, List<ILink>>();
    
    public static TableEntry[] Entries => INSTANCE._guidToTable.Values.ToArray();
    public static Dictionary<ELink, List<ILink>> Links => INSTANCE._links.ToDictionary();

    /// <summary>
    /// Returns the number of indexed table entries
    /// </summary>
    public static int Count => INSTANCE._guidToTable.Count;

    public static bool Contains(Guid pGuid) =>
        INSTANCE._guidToTable.ContainsKey(pGuid);
    
    /// <summary>
    /// Attempts to get the table entry associated with the GUID provided
    /// </summary>
    public static bool TryGet(Guid pGuid, out TableEntry pEntry) => 
        INSTANCE._guidToTable.TryGetValue(pGuid, out pEntry);

    /// <summary>
    /// Register a new table entry with the provided file path and table type
    /// </summary>
    public static void Register(string pPath, ETable pTable)
    {
        if (!StringUtility.TryExtractGuidFromString(pPath, out Guid guid))
            return;
        
        INSTANCE._guidToTable.TryAdd(guid, new TableEntry(pTable, pPath));
    }

    public static void RegisterLink(ELink pTable, ILink pLink)
    {
        if (!INSTANCE._links.ContainsKey(pTable))
            INSTANCE._links.TryAdd(pTable, new List<ILink>());
        if (!INSTANCE._links[pTable].Contains(pLink))
            INSTANCE._links[pTable].Add(pLink);
    }
}