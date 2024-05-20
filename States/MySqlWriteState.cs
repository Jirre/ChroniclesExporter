using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlWriteState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private readonly List<MySqlWriter<IRow>> _writers = new List<MySqlWriter<IRow>>();

    public override void Activate()
    {
        base.Activate();
        Rule header = new Rule("[blue]MySql Write[/]");
        header.Justification = Justify.Left;
        AnsiConsole.Write(header);
        Dictionary<ETable, List<IRow>> tables = new Dictionary<ETable, List<IRow>>();
        foreach (TableEntry entry in TableHandler.Entries)
        {
            if (!tables.ContainsKey(entry.Id))
                tables.Add(entry.Id, new List<IRow>());
            
            if (entry.Row != null)
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
        
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                Dictionary<ETable, ProgressTask> progressTasks = new Dictionary<ETable, ProgressTask>();
                foreach (KeyValuePair<ETable,List<IRow>> kvp in tables)
                {
                    progressTasks.Add(kvp.Key, ctx.AddTask(kvp.Key.ToString(), true, kvp.Value.Count));
                }

                bool isReady = false;
                while (!isReady)
                {
                    isReady = true;
                    foreach (MySqlWriter<IRow> writer in _writers)
                    {
                        progressTasks[(ETable) writer.Id].Value(writer.Progress);
                        if (!writer.IsReady)
                            isReady = false;
                    }
                }

                foreach (MySqlWriter<IRow> writer in _writers)
                {
                    progressTasks[(ETable) writer.Id].Value(tables[(ETable)writer.Id].Count);
                }
            });
    }

    public override void Update()
    {
        foreach (MySqlWriter<IRow> writer in _writers)
        {
            if (!writer.IsReady)
                return;
        }
        
        StateMachine.Goto(EProgramState.MySqlLink);
    }
}
