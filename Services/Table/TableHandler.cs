using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Table;

public class TableHandler
{
    private static TableHandler INSTANCE = new TableHandler();

    private readonly Dictionary<Guid, ETable> _guidToTable = new Dictionary<Guid, ETable>();
    private readonly Dictionary<ETable, Dictionary<Guid, TableEntry>> _tableIndex =
        new Dictionary<ETable, Dictionary<Guid, TableEntry>>();

    /// <summary>
    /// Returns the number of indexed table entries
    /// </summary>
    public static int Count => INSTANCE._guidToTable.Count;
    
    /// <summary>
    /// Attempts to get the Table-Id associated with the table entry GUID provided
    /// </summary>
    public static bool TryGet(Guid pGuid, out ETable pTable) => 
        INSTANCE._guidToTable.TryGetValue(pGuid, out pTable);
    /// <summary>
    /// Attempts to get the table entry associated with the GUID provided
    /// </summary>
    public static bool TryGet(Guid pGuid, out TableEntry pEntry)
    {
        pEntry = null!;
        return INSTANCE._guidToTable.TryGetValue(pGuid, out ETable table) &&
                INSTANCE._tableIndex[table].TryGetValue(pGuid, out pEntry!);
    }

    /// <summary>
    /// Register a new table entry with the provided file path and table type
    /// </summary>
    public static void Register(string pPath, ETable pTable)
    {
        if (!StringUtility.TryExtractGuidFromString(pPath, out Guid guid))
            return;
        
        if (!INSTANCE._tableIndex.ContainsKey(pTable))
            INSTANCE._tableIndex.Add(pTable, new Dictionary<Guid, TableEntry>());
        
        INSTANCE._guidToTable.TryAdd(guid, pTable);
        INSTANCE._tableIndex[pTable].TryAdd(guid, new TableEntry(pTable, pPath));
    }
}