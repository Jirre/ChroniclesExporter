using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Table;

public class TableHandler
{
    private static readonly TableHandler INSTANCE = new TableHandler();

    private readonly Dictionary<Guid, TableEntry> _guidToTable = new Dictionary<Guid, TableEntry>();
    
    public static TableEntry[] Entries => INSTANCE._guidToTable.Values.ToArray();

    /// <summary>
    /// Returns the number of indexed table entries
    /// </summary>
    public static int Count => INSTANCE._guidToTable.Count;
    
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

    public static void InsertRow(IRow pRow)
    {
        if (INSTANCE._guidToTable.TryGetValue(pRow.Id, out TableEntry? table))
            table.Row = pRow;
    }
}