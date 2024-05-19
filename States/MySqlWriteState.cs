using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.States;

public class MySqlWriteState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private readonly List<MySqlWriter<IRow>> _writers = new List<MySqlWriter<IRow>>();

    public override void Activate()
    {
        base.Activate();
        Console.WriteLine("--- MySql Write ---");
        Dictionary<ETable, List<IRow>> tables = new Dictionary<ETable, List<IRow>>();
        foreach (TableEntry entry in TableHandler.Entries)
        {
            if (!tables.ContainsKey(entry.Id))
                tables.Add(entry.Id, new List<IRow>());
            tables[entry.Id].Add(entry.Row);
        }
        foreach (KeyValuePair<ETable,List<IRow>> kvp in tables)
        {
            if (!MySqlHandler.TryGetWriter(kvp.Key, out MySqlWriter<IRow> writer)) continue;
            _writers.Add(writer);
            writer.Prepare(kvp.Value.ToArray());
        }
        foreach (MySqlWriter<IRow> writer in _writers)
            writer.Write();
    }

    public override void Update()
    {
        bool isReady = true;
        foreach (MySqlWriter<IRow> writer in _writers)
        {
            if (!writer.IsReady)
                isReady = false;
            writer.LogStatus();
        }

        if (isReady)
            StateMachine.Goto(EProgramState.MySqlLink);
    }
}
