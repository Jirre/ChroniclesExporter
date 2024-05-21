using ChroniclesExporter.IO;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Log;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlWriteState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    AProgressState<ETable, IWriter>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.MySqlLink;
    protected override ELogCode DefaultErrorCode => ELogCode.MySqlError;
    
    protected override List<Task> BuildHandlers()
    {
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
            Handlers.Add((ETable)writer.Id, writer);
            writer.Prepare(kvp.Value.ToArray());
        }
        
        List<Task> tasks = new List<Task>();
        foreach (KeyValuePair<ETable,IWriter> kvp in Handlers)
        {
            tasks.Add(kvp.Value.Write());
        }

        return tasks;
    }

    protected override void OnHeaderDraw()
    {
        Rule header = new Rule("[blue]Writing to MySql Database[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
